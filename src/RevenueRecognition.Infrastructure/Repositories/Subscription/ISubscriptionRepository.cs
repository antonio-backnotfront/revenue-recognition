namespace RevenueRecognition.Infrastructure.Repositories.Subscription;

using Models.Subscription;
using Models.Contract;

public interface ISubscriptionRepository
{
    public Task<List<Subscription>> GetAllSubscriptionsAsync(CancellationToken cancellationToken);
    public Task<Subscription?> GetSubscriptionByIdAsync(int id, CancellationToken cancellationToken);
    public Task<List<Subscription>> GetAllSubscriptionsByClientIdAsync(int id, CancellationToken cancellationToken);
    public Task<Subscription?> InsertSubscriptionAsync(Subscription subscription, CancellationToken cancellationToken);

    public Task<SubscriptionPayment?> InsertPaymentAsync(
        SubscriptionPayment subscriptionPayment,
        CancellationToken cancellationToken
    );
    
    public Task<DiscountSubscription?> InsertSubscriptionDiscountAsync(
        DiscountSubscription discountSubscription,
        CancellationToken cancellationToken
    );

    public Task<List<SubscriptionPayment>> GetAllPaymentsAsync(CancellationToken cancellationToken);

    public Task<List<SubscriptionPayment>> GetAllPaymentsBySoftwareIdAsync(
        int id,
        CancellationToken cancellationToken
    );
}