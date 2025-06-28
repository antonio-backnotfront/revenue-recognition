using RevenueRecognition.Application.DTOs.Contract;
using RevenueRecognition.Application.DTOs.Payment;

namespace RevenueRecognition.Application.Services.Contract;

public interface IContractService
{
    public Task<CreateContractResponse> CreateContractAsync(CreateContractRequest requestDto,
        CancellationToken cancellationToken);

    public Task<CreatePaymentResponse> IssuePaymentAsync(
        int id,
        CreatePaymentRequest request,
        CancellationToken cancellationToken
    );

    public Task DeleteContractByIdAsync(
        int contractId,
        CancellationToken cancellationToken
    );
}