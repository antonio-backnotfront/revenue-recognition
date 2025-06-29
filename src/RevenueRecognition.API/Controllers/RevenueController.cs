using Microsoft.AspNetCore.Authorization;

namespace RevenueRecognition.API.Controllers;

using RevenueRecognition.Application.Services.Revenue;
using Application.DTOs.Subscription;
using Application.Services.Subscription;
using Application.DTOs.Payment;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin,User")]
[ApiController]
[Route("/api/revenue")]
public class RevenueController : ControllerBase
{
    private readonly ILogger<RevenueController> _logger;
    private readonly IRevenueService _service;

    public RevenueController(
        ILogger<RevenueController> logger,
        IRevenueService service
    )
    {
        _service = service;
        _logger = logger;
    }


    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentRevenueAsync(
        [FromQuery(Name = "softwareId")] string? softwareId,
        [FromQuery(Name = "currency")] string? currency,
        CancellationToken cancellationToken
    )
    {
        int.TryParse(softwareId, out var softwareIdInt);
        decimal revenue = await _service.CalculateRevenueAmountAsync(cancellationToken, softwareIdInt == 0 ? null : softwareIdInt, currency);
        return Ok(new { currentRevenue = revenue });
    }

    [HttpGet("predicted")]
    public async Task<IActionResult> GetPredictedRevenueAsync(
        [FromQuery(Name = "softwareId")] string? softwareId,
        [FromQuery(Name = "currency")] string? currency,
        CancellationToken cancellationToken
    )
    {
        int.TryParse(softwareId, out var softwareIdInt);
        decimal revenue = await _service.CalculatePredictedRevenueAmountAsync(cancellationToken, softwareIdInt == 0 ? null : softwareIdInt, currency);
        return Ok(new { predictedRevenue = revenue });
    }
}