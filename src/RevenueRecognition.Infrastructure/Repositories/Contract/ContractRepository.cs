using Microsoft.EntityFrameworkCore;
using RevenueRecognition.Infrastructure.DAL;

namespace RevenueRecognition.Infrastructure.Repositories.Contract;

using Models.Contract;

public class ContractRepository : IContractRepository
{
    private readonly CompanyDbContext _context;

    public ContractRepository(CompanyDbContext context)
    {
        _context = context;
    }

    public async Task<Contract?> GetContractByIdAsync(
        int id, 
        CancellationToken cancellationToken
        )
    {
        return await _context.Contracts.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Contract?> InsertContractAsync(
        Contract contract, 
        CancellationToken cancellationToken
        )
    {
        return (await _context.Contracts.AddAsync(contract, cancellationToken)).Entity;
    }

    public async Task<ContractPayment?> InsertPaymentAsync(
        ContractPayment payment,
        CancellationToken cancellationToken
    )
    {
        return (await _context.ContractPayments.AddAsync(payment, cancellationToken)).Entity;
    }

    public async Task<List<Contract>> GetAllContractsAsync(
        CancellationToken cancellationToken
        )
    {
        return await _context.Contracts.ToListAsync(cancellationToken);
    }

    public async Task<List<Contract>> GetAllContractsByClientIdAsync(
        int id, 
        CancellationToken cancellationToken
        )
    {
        return await _context.Contracts.Where(c => c.ClientId == id).ToListAsync(cancellationToken);
    }
    
    public async Task<List<Contract>> GetAllContractsBySoftwareIdAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        return await _context.Contracts
                .Include(c => c.SoftwareVersion)
                    .ThenInclude(sv => sv.Software)
            .Where(p => p.SoftwareVersion.SoftwareId == id)
            .ToListAsync(cancellationToken);
    }
}