using RevenueRecognition.Application.DTOs.Subscription;
using RevenueRecognition.Application.Services.Subscription;

namespace RevenueRecognition.API.Controllers;

using Application.DTOs.Contract;
using Application.DTOs.Payment;
using Microsoft.AspNetCore.Mvc;
using Application.Services.Contract;

[ApiController]
[Route("/api/subscriptions")]
public class SubscriptionController : ControllerBase
{
    private readonly ILogger<SubscriptionController> _logger;
    private readonly ISubscriptionService _service;

    public SubscriptionController(
        ILogger<SubscriptionController> logger,
        ISubscriptionService service
    )
    {
        _service = service;
        _logger = logger;
    }


    [HttpPost]
    public async Task<IActionResult> CreateSubscriptionAsync(
        CreateSubscriptionRequest request,
        CancellationToken cancellationToken
    )
    {
        CreateSubscriptionResponse createdSubscription = await _service.CreateSubscriptionOrThrowAsync(request, cancellationToken);
        return StatusCode(201, createdSubscription);
    }

    [HttpPost("{subscriptionId}/issue-payment")]
    public async Task<IActionResult> CreateContractAsync(
        int subscriptionId,
        CreatePaymentRequest request,
        CancellationToken cancellationToken
    )
    {
        CreateSubscriptionPaymentResponse createSubscriptionPayment =
            await _service.IssuePaymentByIdOrThrowAsync(subscriptionId, request, cancellationToken);
        return StatusCode(201, createSubscriptionPayment);
    }
}