namespace RevenueRecognition.Application.Services.Revenue;

using Currency;
using Models.Contract;
using Models.Subscription;
using RevenueRecognition.Infrastructure.Repositories.Contract;
using RevenueRecognition.Infrastructure.Repositories.Subscription;

public class RevenueService : IRevenueService
{
    private readonly ICurrencyConverterService _currencyConverterService;
    private readonly IContractRepository _contractRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;

    public RevenueService(
        ICurrencyConverterService currencyConverterService,
        IContractRepository contractRepository,
        ISubscriptionRepository subscriptionRepository
    )
    {
        _currencyConverterService = currencyConverterService;
        _contractRepository = contractRepository;
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<decimal> CalculateRevenueAmountAsync(
        CancellationToken cancellationToken,
        int? softwareId = null,
        string? currency = null
    )
    {
        var contracts = await GetContractsAsync(softwareId, cancellationToken);
        var payments = await GetSubscriptionPaymentsAsync(softwareId, cancellationToken);

        decimal total = contracts
            .Where(c => string.Equals(c.ContractStatus.Name, Enums.ContractStatus.Paid.ToString(), StringComparison.OrdinalIgnoreCase))
            .Sum(c => c.Price);

        total += payments.Sum(p => p.Amount);

        if (!string.IsNullOrEmpty(currency))
            total = await _currencyConverterService.ConvertAsync(currency, total, cancellationToken);

        return total;
    }

    public async Task<decimal> CalculatePredictedRevenueAmountAsync(
        CancellationToken cancellationToken,
        int? softwareId = null,
        string? currency = null
    )
    {
        var contracts = await GetContractsAsync(softwareId, cancellationToken);
        var payments = await GetSubscriptionPaymentsAsync(softwareId, cancellationToken);
        var subscriptions = await GetSubscriptionsAsync(softwareId, cancellationToken);

        decimal total = contracts
            .Where(c =>
                string.Equals(c.ContractStatus.Name, Enums.ContractStatus.Paid.ToString(), StringComparison.OrdinalIgnoreCase) ||
                string.Equals(c.ContractStatus.Name, Enums.ContractStatus.Active.ToString(), StringComparison.OrdinalIgnoreCase))
            .Sum(c => c.Price);

        total += payments.Sum(p => p.Amount);

        foreach (var subscription in subscriptions)
        {
            var lastPayment = await _subscriptionRepository.GetLastPaymentBySubscriptionIdAsync(subscription.Id, cancellationToken);
            if (lastPayment == null) continue;

            var remainingDays = (new DateTime(DateTime.Now.Year, 12, 31) - lastPayment.PaidAt.Date).Days;
            if (remainingDays <= 0) continue;

            var periodsLeft = Math.Floor((decimal)remainingDays / subscription.RenewalPeriod.Days);
            total += periodsLeft * subscription.Price;
        }

        if (!string.IsNullOrEmpty(currency))
            total = await _currencyConverterService.ConvertAsync(currency, total, cancellationToken);

        return total;
    }

    private async Task<List<Contract>> GetContractsAsync(int? softwareId, CancellationToken ct)
    {
        return softwareId == null
            ? await _contractRepository.GetAllContractsAsync(ct)
            : await _contractRepository.GetAllContractsBySoftwareIdAsync(softwareId.Value, ct);
    }

    private async Task<List<SubscriptionPayment>> GetSubscriptionPaymentsAsync(int? softwareId, CancellationToken ct)
    {
        return softwareId == null
            ? await _subscriptionRepository.GetAllPaymentsAsync(ct)
            : await _subscriptionRepository.GetAllPaymentsBySoftwareIdAsync(softwareId.Value, ct);
    }

    private async Task<List<Subscription>> GetSubscriptionsAsync(int? softwareId, CancellationToken ct)
    {
        return softwareId == null
            ? await _subscriptionRepository.GetAllSubscriptionsAsync(ct)
            : await _subscriptionRepository.GetAllSubscriptionsBySoftwareIdAsync(softwareId.Value, ct);
    }
}
