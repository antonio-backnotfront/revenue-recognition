namespace RevenueRecognition.Application.Services.Subscription;

using DTOs.Payment;
using DTOs.Subscription;
using Models.Subscription;

public interface ISubscriptionService
{
    public Task<CreateSubscriptionResponse> CreateSubscriptionOrThrowAsync(
        CreateSubscriptionRequest request,
        CancellationToken cancellationToken
    );

    public Task<CreateSubscriptionPaymentResponse> IssuePaymentByIdOrThrowAsync(
        int id,
        CreatePaymentRequest request,
        CancellationToken cancellationToken
    );

    public Task<List<Subscription>> GetSubscriptionsByClientIdAsync(
        int clientId,
        CancellationToken cancellationToken
    );

    public Task<List<Subscription>> GetActiveSubscriptionsAsync(
        CancellationToken cancellationToken
    );

    public Task SetSubscriptionSuspendedAsync(
        Subscription subscription,
        CancellationToken cancellationToken
    );

    public Task SetSubscriptionActiveAsync(
        Subscription subscription,
        CancellationToken cancellationToken
    );

    public Task<SubscriptionPayment> GetLastPaymentBySubscriptionIdOrThrow(int subscriptionId,
        CancellationToken cancellationToken);
}