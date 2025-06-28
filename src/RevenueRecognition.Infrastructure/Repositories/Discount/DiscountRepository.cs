using Microsoft.EntityFrameworkCore;
using RevenueRecognition.Infrastructure.DAL;

namespace RevenueRecognition.Infrastructure.Repositories.Discount;

using Models.Discount;

public class DiscountRepository : IDiscountRepository
{
    CompanyDbContext _context;

    public DiscountRepository(CompanyDbContext context)
    {
        _context = context;
    }

    public async Task<Discount?> GetHighestDiscountByDateAsync(DateTime date, CancellationToken cancellationToken)
    {
        return await _context.Discounts
            .Where(c => date >= c.StartDate && date <= c.EndDate)
            .OrderByDescending(c => c.Value)
            .FirstOrDefaultAsync(cancellationToken);
    }
}