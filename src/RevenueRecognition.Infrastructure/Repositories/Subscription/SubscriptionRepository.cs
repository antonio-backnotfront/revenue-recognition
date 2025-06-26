using Microsoft.EntityFrameworkCore;
using RevenueRecognition.Infrastructure.DAL;

namespace RevenueRecognition.Infrastructure.Repositories.Subscription;

using Models.Subscription;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly CompanyDbContext _context;

    public SubscriptionRepository(CompanyDbContext context)
    {
        _context = context;
    }

    public async Task<List<Subscription>> GetAllSubscriptionsAsync(CancellationToken cancellationToken)
    {
        return await _context.Subscriptions
            .ToListAsync(cancellationToken);
    }

    public async Task<Subscription?> GetSubscriptionByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Subscriptions
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<Subscription>> GetAllSubscriptionsByClientIdAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        return await _context.Subscriptions
            .Where(x => x.ClientId == id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Subscription?> InsertSubscriptionAsync(
        Subscription subscription,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    public async Task<SubscriptionPayment?> InsertPaymentAsync(
        SubscriptionPayment subscriptionPayment,
        CancellationToken cancellationToken
    )
    {
        return (await _context.SubscriptionPayments.AddAsync(subscriptionPayment, cancellationToken)).Entity;
    }

    public async Task<List<SubscriptionPayment>> GetAllPaymentsAsync(
        CancellationToken cancellationToken
    )
    {
        return await _context.SubscriptionPayments.ToListAsync(cancellationToken);
    }
    
    public async Task<List<SubscriptionPayment>> GetAllPaymentsBySoftwareIdAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        return await _context.SubscriptionPayments
            .Include(p => p.Subscription)
            .Where(p => p.Subscription.SoftwareId == id)
            .ToListAsync(cancellationToken);
    }
}