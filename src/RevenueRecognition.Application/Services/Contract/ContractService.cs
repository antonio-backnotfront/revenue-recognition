using RevenueRecognition.Application.DTOs.Contract;
using RevenueRecognition.Application.DTOs.Payment;

namespace RevenueRecognition.Application.Services.Contract;

public class ContractService : IContractService
{
    public Task<CreateContractRequest> CreateContractAsync(
        CreateContractRequest dto,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    public Task<CreatePaymentRequest> IssuePaymentAsync(
        CreatePaymentRequest request,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}