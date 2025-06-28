using Microsoft.EntityFrameworkCore;
using RevenueRecognition.Infrastructure.DAL;

namespace RevenueRecognition.Infrastructure.Repositories.Software;

using Models.Software;

public class SoftwareRepository : ISoftwareRepository
{
    private readonly CompanyDbContext _context;

    public SoftwareRepository(CompanyDbContext context)
    {
        _context = context;
    }

    public async Task<SoftwareVersion?> GetSoftwareVersionBySoftwareVersionIdAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        return await _context.SoftwareVersions
            .Include(v => v.Software)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<Software?> GetSoftwareByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Softwares
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }
}