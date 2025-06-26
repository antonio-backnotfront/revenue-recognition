using RevenueRecognition.Application.DTOs.Payment;
using RevenueRecognition.Application.DTOs.Subscription;

namespace RevenueRecognition.Application.Services.Subscription;

public class SubscriptionService : ISubscriptionService
{
    public Task<CreateSubscriptionRequest> CreateSubscriptionAsync(
        CreateSubscriptionRequest request,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    public Task<CreatePaymentRequest> IssuePaymentAsync(
        CreateSubscriptionRequest request,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}