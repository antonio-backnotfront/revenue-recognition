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
        Contract? created = (await _context.Contracts.AddAsync(contract, cancellationToken)).Entity;
        await _context.SaveChangesAsync(cancellationToken);
        
        return created;
    }

    public async Task<ContractPayment?> InsertPaymentAsync(
        ContractPayment payment,
        CancellationToken cancellationToken
    )
    {
        ContractPayment? created = (await _context.ContractPayments.AddAsync(payment, cancellationToken)).Entity;
        payment.Contract.Paid += payment.Amount;
        await _context.SaveChangesAsync(cancellationToken);
        return created;
    }

    public async Task SetContractStatusAsync(Contract contract, int statusId, CancellationToken cancellationToken)
    {
        contract.ContractStatusId = statusId;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<DiscountContract> InsertDiscountContractAsync(DiscountContract discountContract,
        CancellationToken cancellationToken)
    {
        DiscountContract? created =
            (await _context.DiscountContracts.AddAsync(discountContract, cancellationToken)).Entity;
        await _context.SaveChangesAsync(cancellationToken);
        return created;
    }

    public async Task<List<Contract>> GetAllContractsAsync(
        CancellationToken cancellationToken
    )
    {
        return await _context.Contracts
            .Include(c => c.ContractStatus)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Contract>> GetAllContractsBySoftwareIdAsync(int softwareId, CancellationToken cancellationToken)
    {
        return await _context.Contracts
            .Include(c => c.ContractStatus)
            .Where(c => c.SoftwareVersion.SoftwareId == softwareId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Contract>> GetAllContractsByClientIdAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        return await _context.Contracts.Where(c => c.ClientId == id).ToListAsync(cancellationToken);
    }

    public async Task<int?> GetContractStatusIdByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _context.ContractStatuses
            .Where(c => string.Equals(c.Name.ToLower(), name.ToLower()))
            .Select(c => c.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Contract>> GetContractsByClientIdAndSoftwareVersionIdAsync(int clientId,
        int softwareId,
        CancellationToken cancellationToken)
    {
        return await _context.Contracts
            .Include(c => c.ContractStatus)
            .Where(p =>
                p.ClientId == clientId &&
                p.SoftwareVersion.SoftwareId == softwareId
            ).ToListAsync(cancellationToken);
    }

    public async Task DeleteContractByIdAsync(int id, CancellationToken cancellationToken)
    {
        Contract contract = await _context.Contracts
            .Include(c => c.ContractPayments)
            .Include(c => c.DiscountContracts)
            .FirstAsync(c => c.Id == id, cancellationToken);
        _context.ContractPayments.RemoveRange(contract.ContractPayments);
        _context.DiscountContracts.RemoveRange(contract.DiscountContracts);
        _context.Contracts.Remove(contract);
        await _context.SaveChangesAsync(cancellationToken);
    }
}