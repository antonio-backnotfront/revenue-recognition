namespace RevenueRecognition.Application.Services.Subscription;

using RevenueRecognition.Application.DTOs.Client;
using Exceptions;
using Client;
using Discount;
using Software;
using Infrastructure.Repositories.UnitOfWork;
using RevenueRecognition.Infrastructure.Repositories.Subscription;
using DTOs.Payment;
using DTOs.Subscription;
using Models.Subscription;
using Models.Software;
using Models.Discount;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _repository;
    private readonly ISoftwareService _softwareService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClientService _clientService;
    private readonly IDiscountService _discountService;

    public SubscriptionService(
        ISubscriptionRepository subscriptionRepository,
        ISoftwareService softwareService,
        IUnitOfWork unitOfWork,
        IClientService clientService,
        IDiscountService discountService
    )
    {
        _softwareService = softwareService;
        _repository = subscriptionRepository;
        _unitOfWork = unitOfWork;
        _clientService = clientService;
        _discountService = discountService;
    }



    public async Task<CreateSubscriptionResponse> CreateSubscriptionOrThrowAsync(
        CreateSubscriptionRequest request,
        CancellationToken cancellationToken
    )
    {
        Software software = await _softwareService.GetSoftwareByIdOrThrowAsync(
            request.SoftwareId,
            cancellationToken
        );

        GetClientResponse client = await _clientService.GetClientByIdOrThrowAsync(
            request.ClientId,
            cancellationToken
        );

        int activeSubscriptionStatus = await GetSubscriptionStatusIdByNameOrThrowAsync(
            Enums.SubscriptionStatus.Active,
            cancellationToken
        );

        bool alreadyHasActiveSubscription = (await GetSubscriptionsByClientIdAsync(
                client.Id,
                cancellationToken
            ))
            .Any(s =>
                s.SubscriptionStatusId == activeSubscriptionStatus &&
                s.SoftwareId == software.Id);
        if (alreadyHasActiveSubscription)
            throw new AlreadyExistsException("Client already has an active subscription for this software.");

        RenewalPeriod renewalPeriod =
            await GetRenewalPeriodByIdOrThrowAsync(request.RenewalPeriodId, cancellationToken);

        await _unitOfWork.StartTransactionAsync(cancellationToken);
        try
        {
            decimal basePricePerMonth = software.Cost / 12;
            decimal renewalPayment =
                await CalculateDiscountedRenewalPriceAsync(basePricePerMonth, client.IsLoyal, cancellationToken);
            Subscription toInsertSubscription = new Subscription()
            {
                SoftwareId = software.Id,
                ClientId = client.Id,
                SubscriptionStatusId = activeSubscriptionStatus,
                RenewalPeriodId = renewalPeriod.Id,
                Price = renewalPayment,
                RegisterDate = DateTime.Now,
            };
            Subscription? createdSubscription =
                await _repository.InsertSubscriptionAsync(toInsertSubscription, cancellationToken);
            if (createdSubscription == null)
                throw new Exception($"Unable to create subscription for {request}.");

            decimal firstPayment =
                await CalculateDiscountedFirstPriceAsync(basePricePerMonth, client.IsLoyal, cancellationToken);
            await InsertPaymentBySubscriptionIdOrThrowAsync(createdSubscription.Id, firstPayment, cancellationToken);

            await AttachApplicableDiscountsOrThrowAsync(createdSubscription.Id, client.IsLoyal, cancellationToken);
            await _clientService.SetIsClientLoyalByIdOrThrowAsync(client.Id, true, cancellationToken);

            
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return new CreateSubscriptionResponse()
            {
                Id = createdSubscription.Id,
                SoftwareId = createdSubscription.SoftwareId,
                ClientId = createdSubscription.ClientId,
                SubscriptionStatusId = createdSubscription.SubscriptionStatusId,
                RenewalPeriodId = createdSubscription.RenewalPeriodId,
                Price = createdSubscription.Price,
                RegisterDate = createdSubscription.RegisterDate
            };
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    // user must pay on the first day of the new renewal period
    public async Task<CreateSubscriptionPaymentResponse> IssuePaymentByIdOrThrowAsync(
        int id,
        CreatePaymentRequest request,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    private async Task InsertPaymentBySubscriptionIdOrThrowAsync(int id, decimal amount,
        CancellationToken cancellationToken)
    {
        _ = await _repository.InsertPaymentAsync(
            new SubscriptionPayment()
            {
                SubscriptionId = id,
                Amount = amount,
                PaidAt = DateTime.Now,
            }, cancellationToken
        ) ?? throw new Exception("Unable to insert subscription payment.");
    }

    public async Task<List<Subscription>> GetSubscriptionsByClientIdAsync(
        int clientId,
        CancellationToken cancellationToken
    )
    {
        return await _repository.GetAllSubscriptionsByClientIdAsync(clientId, cancellationToken);
    }

    private async Task<int> GetSubscriptionStatusIdByNameOrThrowAsync(
        Enums.SubscriptionStatus status,
        CancellationToken ct
    )
    {
        var id = await _repository.GetSubscriptionStatusIdByNameAsync(status.ToString(), ct);
        return id ?? throw new NotFoundException($"Error while getting contract status '{status}'");
    }

    private async Task<RenewalPeriod> GetRenewalPeriodByIdOrThrowAsync(
        int id,
        CancellationToken ct
    )
    {
        RenewalPeriod? renewalPeriod = await _repository.GetRenewalPeriodByIdAsync(id, ct);
        return renewalPeriod ?? throw new NotFoundException($"Renewal period with id {id} not found.");
    }

    private async Task<decimal> CalculateDiscountedFirstPriceAsync(decimal basePrice, bool isLoyal,
        CancellationToken ct)
    {
        var discount = await _discountService.GetDiscountByDateAsync(DateTime.Now, ct);
        var loyaltyDiscount = isLoyal ? await _discountService.GetLoyalClientDiscountAsync(ct) : null;

        decimal total = basePrice;
        if (discount != null) total *= (100 - discount.Value) / 100;
        if (loyaltyDiscount != null) total *= (100 - loyaltyDiscount.Value) / 100;
        return total;
    }

    private async Task<decimal> CalculateDiscountedRenewalPriceAsync(decimal basePrice, bool isLoyal,
        CancellationToken ct)
    {
        var loyaltyDiscount = isLoyal ? await _discountService.GetLoyalClientDiscountAsync(ct) : null;
        if (loyaltyDiscount != null) return basePrice * (100 - loyaltyDiscount.Value) / 100;
        return basePrice;
    }

    private async Task AttachApplicableDiscountsOrThrowAsync(int subscriptionId, bool isLoyal, CancellationToken ct)
    {
        var loyaltyDiscount = isLoyal ? await _discountService.GetLoyalClientDiscountAsync(ct) : null;

        if (loyaltyDiscount != null)
            await _repository.InsertSubscriptionDiscountAsync(
                new DiscountSubscription { DiscountId = loyaltyDiscount.Id, SubscriptionId = subscriptionId }, ct);
    }
}