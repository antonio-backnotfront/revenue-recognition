using RevenueRecognition.Application.DTOs.Contract;
using RevenueRecognition.Application.DTOs.Payment;

namespace RevenueRecognition.Application.Services.Contract;

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
}