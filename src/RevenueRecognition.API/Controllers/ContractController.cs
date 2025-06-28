using RevenueRecognition.Application.DTOs.Contract;
using RevenueRecognition.Application.DTOs.Payment;

namespace RevenueRecognition.API.Controllers;

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
        CreateContractResponse createdContract = await _service.CreateContractAsync(request, cancellationToken);
        return StatusCode(201, createdContract);
    }
    
    [HttpPost("{contractId}/issue-payment")]
    public async Task<IActionResult> CreateContractAsync(
        int contractId,
        CreatePaymentRequest request,
        CancellationToken cancellationToken
    )
    {
        CreatePaymentResponse createdPayment = await _service.IssuePaymentAsync(contractId, request, cancellationToken);
        return StatusCode(201, createdPayment);
    }
}