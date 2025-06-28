using RevenueRecognition.Infrastructure.Repositories.Subscription;

namespace RevenueRecognition.Application.Services.Subscription;

using RevenueRecognition.Application.DTOs.Payment;
using RevenueRecognition.Application.DTOs.Subscription;
using Models.Subscription;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _repository;

    public SubscriptionService(ISubscriptionRepository subscriptionRepository)
    {
        _repository = subscriptionRepository;
    }

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

    public Task<List<Subscription>> GetSubscriptionsByClientIdAsync(
        int clientId,
        CancellationToken cancellationToken
    )
    {
        return _repository.GetAllSubscriptionsByClientIdAsync(clientId, cancellationToken);
    }
}