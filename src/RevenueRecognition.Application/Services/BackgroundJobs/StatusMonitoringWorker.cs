using Microsoft.Extensions.Logging;

namespace RevenueRecognition.Application.Services.BackgroundJobs;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Contract;
using Subscription;
using Models.Contract;
using Models.Subscription;

public class StatusMonitoringWorker : BackgroundService
{
    // we need to use scope factory becasue BackgroundService is singleton by its nature,
    // therefore we need to parse our scoped dependencies such as services, dbcontexts etc.
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<StatusMonitoringWorker> _logger;

    public StatusMonitoringWorker(
        ILogger<StatusMonitoringWorker> logger,
        IServiceScopeFactory scopeFactory
    )
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var contractService = scope.ServiceProvider.GetRequiredService<IContractService>();
                var subscriptionService = scope.ServiceProvider.GetRequiredService<ISubscriptionService>();

                List<Contract> contracts =
                    await contractService.GetAllActiveContractsAsync(stoppingToken);
                foreach (Contract contract in contracts)
                {
                    Console.WriteLine($"{contract.Id}");
                    
                    if (contract.EndDate < DateTime.Now && contract.Paid != contract.Price)
                    {
                        await contractService.SetContractCancelledAsync(contract, stoppingToken);
                    }
                }

                List<Subscription> subscriptions =
                    await subscriptionService.GetActiveSubscriptionsAsync(stoppingToken);
                foreach (Subscription subscription in subscriptions)
                {
                    SubscriptionPayment lastPayment =
                        await subscriptionService.GetLastPaymentBySubscriptionIdOrThrow(subscription.Id, stoppingToken);
                    if (lastPayment.PaidAt.AddDays(subscription.RenewalPeriod.Days + 3) < DateTime.Today)
                        await subscriptionService.SetSubscriptionSuspendedAsync(subscription, stoppingToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}