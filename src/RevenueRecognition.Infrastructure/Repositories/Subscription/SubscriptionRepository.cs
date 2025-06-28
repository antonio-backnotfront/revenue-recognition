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
            .Include(s => s.Software)
            .ToListAsync(cancellationToken);
    }

    public async Task<Subscription?> GetSubscriptionByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Subscriptions
            .Include(s => s.Software)
            .Include(s => s.RenewalPeriod)
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

    public async Task<RenewalPeriod?> GetRenewalPeriodByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.RenewalPeriods
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public Task ChangeSubscriptionStatusAsync(Subscription subscription, int status, CancellationToken cancellationToken)
    {
        subscription.SubscriptionStatusId = status;
        return _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int?> GetSubscriptionStatusIdByNameAsync(string name, CancellationToken cancellationToken)
    { 
        return await _context.SubscriptionStatuses
            .Where(c => string.Equals(c.Name.ToLower(), name.ToLower()))
            .Select(c => c.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<SubscriptionPayment>> GetAllPaymentsAsync(
        CancellationToken cancellationToken
    )
    {
        return await _context.SubscriptionPayments.ToListAsync(cancellationToken);
    }
    
    public async Task<SubscriptionPayment?> GetLastPaymentBySubscriptionIdAsync(
        int subscriptionId,
        CancellationToken cancellationToken
    )
    {
        return await _context.SubscriptionPayments
            .Where(p => p.SubscriptionId == subscriptionId)
            .OrderByDescending(e => e.PaidAt)
            .FirstOrDefaultAsync(cancellationToken);
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