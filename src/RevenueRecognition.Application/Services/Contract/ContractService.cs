using RevenueRecognition.Infrastructure.Repositories.UnitOfWork;

namespace RevenueRecognition.Application.Services.Contract;

using RevenueRecognition.Application.DTOs.Client;
using Software;
using RevenueRecognition.Models.Software;
using DTOs.Payment;
using Exceptions;
using Client;
using Discount;
using Subscription;
using DTOs.Contract;
using RevenueRecognition.Infrastructure.Repositories.Contract;
using RevenueRecognition.Models.Contract;
using Models.Subscription;
using Models.Discount;

public class ContractService : IContractService
{
    private readonly IContractRepository _repository;
    private readonly IDiscountService _discountService;
    private readonly IClientService _clientService;
    private readonly ISoftwareService _softwareService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly IUnitOfWork _unitOfWork;

    public ContractService(
        IContractRepository repository,
        IDiscountService discountService,
        IClientService clientService,
        ISubscriptionService subscriptionService,
        ISoftwareService softwareService,
        IUnitOfWork unitOfWork
    )
    {
        _unitOfWork = unitOfWork;
        _softwareService = softwareService;
        _subscriptionService = subscriptionService;
        _repository = repository;
        _discountService = discountService;
        _clientService = clientService;
    }

    public async Task<CreateContractResponse> CreateContractOrThrowAsync(CreateContractRequest requestDto,
        CancellationToken cancellationToken)
    {
        var client = await _clientService.GetClientByIdOrThrowAsync(requestDto.ClientId, cancellationToken);
        var softwareVersion =
            await _softwareService.GetSoftwareVersionBySoftwareVersionIdOrThrowAsync(requestDto.SoftwareVersionId,
                cancellationToken);

        await ValidateNoActiveContractOrSubscription(client.Id, softwareVersion.SoftwareId, cancellationToken);
        ValidateContractDates(requestDto.StartDate, requestDto.EndDate);
        ValidateSupportYears(requestDto.YearsOfSupport);

        var totalPrice = await CalculateDiscountedPriceAsync(softwareVersion.Software.Cost, client.IsLoyal,
            requestDto.YearsOfSupport, cancellationToken);
        var statusId = await GetContractStatusIdOrThrowAsync(Enums.ContractStatus.Active, cancellationToken);

        var contract = new Contract
        {
            SoftwareVersionId = softwareVersion.Id,
            ClientId = client.Id,
            ContractStatusId = statusId,
            StartDate = requestDto.StartDate,
            EndDate = requestDto.EndDate,
            YearsOfSupport = requestDto.YearsOfSupport,
            Price = totalPrice,
            Paid = 0
        };

        await _unitOfWork.StartTransactionAsync(cancellationToken);
        try
        {
            Contract? created = await _repository.InsertContractAsync(contract, cancellationToken);
            if (created == null)
                throw new Exception("Couldn't create a contract.");

            await AttachApplicableDiscountsAsync(created.Id, client.IsLoyal, cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return new CreateContractResponse
            {
                Id = created.Id,
                ClientId = client.Id,
                SoftwareVersionId = softwareVersion.Id,
                StartDate = requestDto.StartDate,
                EndDate = requestDto.EndDate,
                YearsOfSupport = requestDto.YearsOfSupport,
            };
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }


    public async Task<CreateContractPaymentResponse> IssuePaymentByIdOrThrowAsync(
        int contractId,
        CreatePaymentRequest request,
        CancellationToken cancellationToken
    )
    {
        Contract contract = await GetValidContractOrThrowAsync(contractId, cancellationToken);

        ValidatePaymentAllowedOrThrow(contract, request.Amount);

        await _unitOfWork.StartTransactionAsync(cancellationToken);
        try
        {
            await ChangeStatusIfFullyPaidAsync(contract, request.Amount, cancellationToken);

            ContractPayment payment = await _repository.InsertPaymentAsync(new ContractPayment
            {
                ContractId = contract.Id,
                Amount = request.Amount,
                PaidAt = DateTime.Now
            }, cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return new CreateContractPaymentResponse
            {
                Id = payment.Id,
                ContractId = contract.Id,
                Amount = request.Amount,
                DateOfPayment = payment.PaidAt
            };
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }


    public async Task DeleteContractByIdOrThrowAsync(int contractId, CancellationToken cancellationToken)
    {
        await _unitOfWork.StartTransactionAsync(cancellationToken);
        try
        {
            await _repository.DeleteContractByIdAsync(contractId, cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
        }
    }

    private async Task ValidateNoActiveContractOrSubscription(int clientId, int softwareVersionId, CancellationToken ct)
    {
        var contracts =
            await _repository.GetContractsByClientIdAndSoftwareVersionIdAsync(clientId, softwareVersionId, ct);
        if (contracts.Any(IsContractActiveOrRecentPaid))
            throw new AlreadyExistsException("Client already has an active contract for this software.");

        var subscriptions = await _subscriptionService.GetSubscriptionsByClientIdAsync(clientId, ct);
        Console.WriteLine($"{string.Join(",", subscriptions)}");
        if (subscriptions.Any(s =>
                s.SubscriptionStatus.Name.Equals(Enums.SubscriptionStatus.Active.ToString(),
                    StringComparison.OrdinalIgnoreCase)
                &&
                s.SoftwareId == softwareVersionId))
            throw new AlreadyExistsException("Client already has an active subscription for this software.");
    }

    private bool IsContractActiveOrRecentPaid(Contract contract)
    {
        return contract.ContractStatus.Name.Equals(Enums.ContractStatus.Active.ToString(),
                   StringComparison.OrdinalIgnoreCase) ||
               (contract.ContractStatus.Name.Equals(Enums.ContractStatus.Paid.ToString(),
                    StringComparison.OrdinalIgnoreCase)
                && contract.EndDate.AddYears(contract.YearsOfSupport) > DateTime.Now);
    }

    private void ValidateContractDates(DateTime start, DateTime end)
    {
        if (end < start)
            throw new BadRequestException("Start date cannot be before end date.");
        if (start < DateTime.Now)
            throw new BadRequestException("Start date must be in the future.");
        if ((end - start).Days is < 3 or > 30)
            throw new BadRequestException("Duration must be between 3 and 30 days.");
    }

    private void ValidateSupportYears(int years)
    {
        if (years < 1 || years > 4)
            throw new BadRequestException("Support years must be between 1 and 4.");
    }

    private async Task<decimal> CalculateDiscountedPriceAsync(decimal basePrice, bool isLoyal, int years,
        CancellationToken ct)
    {
        var discount = await _discountService.GetDiscountByDateAsync(DateTime.Now, ct);
        var loyaltyDiscount = isLoyal ? await _discountService.GetLoyalClientDiscountAsync(ct) : null;

        decimal total = basePrice + ((years - 1) * 1000);
        if (discount != null) total *= (100 - discount.Value) / 100;
        if (loyaltyDiscount != null) total *= (100 - loyaltyDiscount.Value) / 100;
        return total;
    }

    private async Task AttachApplicableDiscountsAsync(int contractId, bool isLoyal, CancellationToken ct)
    {
        var discount = await _discountService.GetDiscountByDateAsync(DateTime.Now, ct);
        var loyaltyDiscount = isLoyal ? await _discountService.GetLoyalClientDiscountAsync(ct) : null;

        if (discount != null)
            await _repository.InsertDiscountContractAsync(
                new DiscountContract { DiscountId = discount.Id, ContractId = contractId }, ct);

        if (loyaltyDiscount != null)
            await _repository.InsertDiscountContractAsync(
                new DiscountContract { DiscountId = loyaltyDiscount.Id, ContractId = contractId }, ct);
    }

    private async Task<int> GetContractStatusIdOrThrowAsync(Enums.ContractStatus status, CancellationToken ct)
    {
        var id = await _repository.GetContractStatusIdByNameAsync(status.ToString(), ct);
        return id ?? throw new Exception($"Error while getting contract status '{status}'");
    }

    private async Task<Contract> GetValidContractOrThrowAsync(int contractId, CancellationToken ct)
    {
        var contract = await _repository.GetContractByIdAsync(contractId, ct);
        if (contract == null)
            throw new NotFoundException($"Contract with id {contractId} not found.");

        var activeStatusId =
            await _repository.GetContractStatusIdByNameAsync(Enums.ContractStatus.Active.ToString(), ct);
        if (activeStatusId == null)
            throw new Exception("Error while getting 'Active' contract status.");

        if (contract.ContractStatusId != activeStatusId)
            throw new AlreadyExistsException("Contract is not active and cannot be paid.");

        return contract;
    }

    private void ValidatePaymentAllowedOrThrow(Contract contract, decimal amount)
    {
        if (contract.Paid + amount > contract.Price)
            throw new BadRequestException(
                $"Payment exceeds remaining amount. Remaining: {contract.Price - contract.Paid}.");
    }

    private async Task ChangeStatusIfFullyPaidAsync(Contract contract, decimal amount, CancellationToken ct)
    {
        var totalPaid = contract.Paid + amount;
        if (totalPaid < contract.Price) return;

        var paidStatusId = await _repository.GetContractStatusIdByNameAsync(Enums.ContractStatus.Paid.ToString(), ct);
        if (paidStatusId == null)
            throw new Exception("Error while getting 'Paid' contract status.");

        await _repository.SetContractStatusAsync(contract, paidStatusId.Value, ct);

        if (!await _clientService.IsClientLoyalByIdOrThrowAsync(contract.ClientId, ct))
        {
            await _clientService.SetIsClientLoyalByIdOrThrowAsync(contract.ClientId, true, ct);
        }
    }
}