namespace RevenueRecognition.Application.Services.Contract;
using RevenueRecognition.Application.DTOs.Contract;
using DTOs.Payment;
using Models.Contract;


public interface IContractService
{
    public Task<CreateContractResponse> CreateContractOrThrowAsync(CreateContractRequest requestDto,
        CancellationToken cancellationToken);

    public Task<CreateContractPaymentResponse> IssuePaymentByIdOrThrowAsync(
        int id,
        CreatePaymentRequest request,
        CancellationToken cancellationToken
    );

    public Task DeleteContractByIdOrThrowAsync(
        int contractId,
        CancellationToken cancellationToken
    );

    public Task<List<Contract>> GetAllActiveContractsAsync(CancellationToken cancellationToken);
    public Task SetContractPaidAsync(Contract contract, CancellationToken cancellationToken);
    public Task SetContractCancelledAsync(Contract contract, CancellationToken cancellationToken);
    public Task SetContractActiveAsync(Contract contract, CancellationToken cancellationToken);
}