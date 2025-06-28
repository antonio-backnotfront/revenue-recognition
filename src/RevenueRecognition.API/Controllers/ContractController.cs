namespace RevenueRecognition.API.Controllers;

using Application.DTOs.Contract;
using Application.DTOs.Payment;
using Microsoft.AspNetCore.Mvc;
using Application.Services.Contract;

[ApiController]
[Route("/api/contracts")]
public class ContractController : ControllerBase
{
    private readonly ILogger<ContractController> _logger;
    private readonly IContractService _service;

    public ContractController(
        ILogger<ContractController> logger,
        IContractService service
    )
    {
        _service = service;
        _logger = logger;
    }


    [HttpPost]
    public async Task<IActionResult> CreateContractAsync(
        CreateContractRequest request,
        CancellationToken cancellationToken
    )
    {
        CreateContractResponse createdContract = await _service.CreateContractOrThrowAsync(request, cancellationToken);
        return StatusCode(201, createdContract);
    }

    [HttpPost("{contractId}/issue-payment")]
    public async Task<IActionResult> CreateContractAsync(
        int contractId,
        CreatePaymentRequest request,
        CancellationToken cancellationToken
    )
    {
        CreateContractPaymentResponse createdContractPayment =
            await _service.IssuePaymentByIdOrThrowAsync(contractId, request, cancellationToken);
        return StatusCode(201, createdContractPayment);
    }

    [HttpDelete("{contractId}")]
    public async Task<IActionResult> DeleteContractAsync(
        int contractId,
        CancellationToken cancellationToken
    )
    {
        await _service.DeleteContractByIdOrThrowAsync(contractId, cancellationToken);
        return StatusCode(204);
    }
}