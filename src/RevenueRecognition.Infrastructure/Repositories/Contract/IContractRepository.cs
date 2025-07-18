namespace RevenueRecognition.Infrastructure.Repositories.Contract;

using Models.Contract;

public interface IContractRepository
{
    public Task<Contract?> GetContractByIdAsync(int id, CancellationToken cancellationToken);
    public Task<Contract?> InsertContractAsync(Contract contract, CancellationToken cancellationToken);
    public Task<ContractPayment?> InsertPaymentAsync(ContractPayment payment, CancellationToken cancellationToken);

    public Task SetContractStatusAsync(Contract contract, int statusId, CancellationToken cancellationToken);
    
    public Task<DiscountContract> InsertDiscountContractAsync(DiscountContract discountContract,
        CancellationToken cancellationToken);

    public Task<List<Contract>> GetAllContractsAsync(CancellationToken cancellationToken);
    public Task<List<Contract>> GetAllContractsBySoftwareIdAsync(int softwareId, CancellationToken cancellationToken);
    public Task<List<Contract>> GetAllContractsByClientIdAsync(int id, CancellationToken cancellationToken);
    public Task<int?> GetContractStatusIdByNameAsync(string name, CancellationToken cancellationToken);

    public Task<List<Contract>> GetContractsByClientIdAndSoftwareVersionIdAsync(int clientId,
        int softwareId,
        CancellationToken cancellationToken);
    
    public Task DeleteContractByIdAsync(int id, CancellationToken cancellationToken);
}