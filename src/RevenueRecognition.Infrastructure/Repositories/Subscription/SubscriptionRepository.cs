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
            .Include(x => x.SubscriptionStatus)
            .Where(x => x.ClientId == id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Subscription?> InsertSubscriptionAsync(
        Subscription subscription,
        CancellationToken cancellationToken
    )
    {
        Subscription created =
            (await _context.Subscriptions.AddAsync(subscription, cancellationToken)).Entity;
        await _context.SaveChangesAsync(cancellationToken);
        return created;
    }

    public async Task<SubscriptionPayment?> InsertPaymentAsync(
        SubscriptionPayment subscriptionPayment,
        CancellationToken cancellationToken
    )
    {
        SubscriptionPayment created =
            (await _context.SubscriptionPayments.AddAsync(subscriptionPayment, cancellationToken)).Entity;
        await _context.SaveChangesAsync(cancellationToken);
        return created;
    }

    public async Task<DiscountSubscription?> InsertSubscriptionDiscountAsync(DiscountSubscription discountSubscription,
        CancellationToken cancellationToken)
    {
        DiscountSubscription? created = (await
            _context.DiscountSubscriptions.AddAsync(discountSubscription, cancellationToken)).Entity;
        await _context.SaveChangesAsync(cancellationToken);
        return created;
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