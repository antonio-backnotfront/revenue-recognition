namespace RevenueRecognition.Application.Services.Subscription;

using DTOs.Payment;
using DTOs.Subscription;
using Models.Subscription;

public interface ISubscriptionService
{
    public Task<CreateSubscriptionRequest> CreateSubscriptionAsync(
        CreateSubscriptionRequest request,
        CancellationToken cancellationToken
    );

    public Task<CreatePaymentRequest> IssuePaymentAsync(
        CreateSubscriptionRequest request,
        CancellationToken cancellationToken
    );

    public Task<List<Subscription>> GetSubscriptionsByClientIdAsync(
        int clientId,
        CancellationToken cancellationToken
    );
}