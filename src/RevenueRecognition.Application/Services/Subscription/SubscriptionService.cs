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
        IDiscountService discountService)
    {
        _repository = subscriptionRepository;
        _softwareService = softwareService;
        _unitOfWork = unitOfWork;
        _clientService = clientService;
        _discountService = discountService;
    }

    public async Task<CreateSubscriptionResponse> CreateSubscriptionOrThrowAsync(
        CreateSubscriptionRequest request,
        CancellationToken cancellationToken)
    {
        var software = await _softwareService.GetSoftwareByIdOrThrowAsync(request.SoftwareId, cancellationToken);
        var client = await _clientService.GetClientByIdOrThrowAsync(request.ClientId, cancellationToken);
        var renewalPeriod = await GetRenewalPeriodByIdOrThrowAsync(request.RenewalPeriodId, cancellationToken);
        var activeStatusId = await GetSubscriptionStatusIdByNameOrThrowAsync(Enums.SubscriptionStatus.Active, cancellationToken);

        bool hasActive = (await GetSubscriptionsByClientIdAsync(client.Id, cancellationToken))
            .Any(s => s.SoftwareId == software.Id && s.SubscriptionStatusId == activeStatusId);

        if (hasActive)
            throw new AlreadyExistsException("Client already has an active subscription for this software.");

        decimal monthlyPrice = software.Cost / 12m;
        decimal basePrice = monthlyPrice * Math.Floor(renewalPeriod.Days / 30m);
        var renewalPrice = await CalculateDiscountedRenewalPriceAsync(basePrice, client.IsLoyal, cancellationToken);
        var firstPayment = await CalculateDiscountedFirstPriceAsync(basePrice, client.IsLoyal, cancellationToken);

        var subscription = new Subscription
        {
            SoftwareId = software.Id,
            ClientId = client.Id,
            RenewalPeriodId = renewalPeriod.Id,
            SubscriptionStatusId = activeStatusId,
            Price = renewalPrice,
            RegisterDate = DateTime.Now
        };

        await _unitOfWork.StartTransactionAsync(cancellationToken);

        try
        {
            var created = await _repository.InsertSubscriptionAsync(subscription, cancellationToken)
                          ?? throw new Exception("Unable to create subscription.");

            await InsertPaymentBySubscriptionIdOrThrowAsync(created.Id, firstPayment, cancellationToken);
            await AttachApplicableDiscountsOrThrowAsync(created.Id, client.IsLoyal, cancellationToken);
            await _clientService.SetIsClientLoyalByIdOrThrowAsync(client.Id, true, cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return new CreateSubscriptionResponse
            {
                Id = created.Id,
                SoftwareId = created.SoftwareId,
                ClientId = created.ClientId,
                RenewalPeriodId = created.RenewalPeriodId,
                SubscriptionStatusId = created.SubscriptionStatusId,
                Price = created.Price,
                RegisterDate = created.RegisterDate
            };
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<CreateSubscriptionPaymentResponse> IssuePaymentByIdOrThrowAsync(
        int subscriptionId,
        CreatePaymentRequest request,
        CancellationToken cancellationToken)
    {
        var subscription = await _repository.GetSubscriptionByIdAsync(subscriptionId, cancellationToken)
                            ?? throw new NotFoundException($"Subscription with id {subscriptionId} not found.");

        int activeStatusId = await GetSubscriptionStatusIdByNameOrThrowAsync(Enums.SubscriptionStatus.Active, cancellationToken);
        if (subscription.SubscriptionStatusId != activeStatusId)
            throw new BadRequestException("Subscription is not active.");

        var lastPayment = await _repository.GetLastPaymentBySubscriptionIdAsync(subscriptionId, cancellationToken)
                         ?? throw new Exception("Missing initial payment record.");

        var renewalDays = subscription.RenewalPeriod.Days;
        var nextDueDate = lastPayment.PaidAt.Date.AddDays(renewalDays);
        var today = DateTime.Today;

        if (today < nextDueDate)
            throw new BadRequestException("Too early to pay for the next renewal period.");

        if (today > nextDueDate)
        {
            var suspendedId = await GetSubscriptionStatusIdByNameOrThrowAsync(Enums.SubscriptionStatus.Suspended, cancellationToken);
            await _repository.ChangeSubscriptionStatusAsync(subscription, suspendedId, cancellationToken);
            throw new BadRequestException("Subscription was suspended due to missed payment.");
        }

        if (request.Amount != subscription.Price)
            throw new BadRequestException($"Incorrect payment amount. Expected: {subscription.Price}");

        var payment = await InsertPaymentBySubscriptionIdOrThrowAsync(subscriptionId, request.Amount, cancellationToken);

        return new CreateSubscriptionPaymentResponse
        {
            Id = payment.Id,
            SubscriptionId = subscription.Id,
            Amount = payment.Amount,
            DateOfPayment = payment.PaidAt
        };
    }

    public async Task<List<Subscription>> GetSubscriptionsByClientIdAsync(
        int clientId,
        CancellationToken cancellationToken)
    {
        return await _repository.GetAllSubscriptionsByClientIdAsync(clientId, cancellationToken);
    }

    private async Task<SubscriptionPayment> InsertPaymentBySubscriptionIdOrThrowAsync(
        int subscriptionId,
        decimal amount,
        CancellationToken ct)
    {
        return await _repository.InsertPaymentAsync(new SubscriptionPayment
        {
            SubscriptionId = subscriptionId,
            Amount = amount,
            PaidAt = DateTime.Today
        }, ct) ?? throw new Exception("Failed to insert subscription payment.");
    }

    private async Task AttachApplicableDiscountsOrThrowAsync(int subscriptionId, bool isLoyal, CancellationToken ct)
    {
        var loyaltyDiscount = isLoyal ? await _discountService.GetLoyalClientDiscountAsync(ct) : null;
        if (loyaltyDiscount != null)
        {
            await _repository.InsertSubscriptionDiscountAsync(new DiscountSubscription
            {
                SubscriptionId = subscriptionId,
                DiscountId = loyaltyDiscount.Id
            }, ct);
        }
    }

    private async Task<int> GetSubscriptionStatusIdByNameOrThrowAsync(
        Enums.SubscriptionStatus status,
        CancellationToken ct)
    {
        var id = await _repository.GetSubscriptionStatusIdByNameAsync(status.ToString(), ct);
        return id ?? throw new NotFoundException($"Subscription status '{status}' not found.");
    }

    private async Task<RenewalPeriod> GetRenewalPeriodByIdOrThrowAsync(int id, CancellationToken ct)
    {
        var period = await _repository.GetRenewalPeriodByIdAsync(id, ct);
        return period ?? throw new NotFoundException($"Renewal period with id {id} not found.");
    }

    private async Task<decimal> CalculateDiscountedFirstPriceAsync(decimal basePrice, bool isLoyal, CancellationToken ct)
    {
        var discount = await _discountService.GetDiscountByDateAsync(DateTime.Now, ct);
        var loyaltyDiscount = isLoyal ? await _discountService.GetLoyalClientDiscountAsync(ct) : null;

        decimal total = basePrice;
        if (discount != null) total *= (100 - discount.Value) / 100;
        if (loyaltyDiscount != null) total *= (100 - loyaltyDiscount.Value) / 100;
        return total;
    }

    private async Task<decimal> CalculateDiscountedRenewalPriceAsync(decimal basePrice, bool isLoyal, CancellationToken ct)
    {
        var loyaltyDiscount = isLoyal ? await _discountService.GetLoyalClientDiscountAsync(ct) : null;
        return loyaltyDiscount != null ? basePrice * (100 - loyaltyDiscount.Value) / 100 : basePrice;
    }
}
