namespace RevenueRecognition.Application.Services.Revenue;

public interface IRevenueService
{
    public Task<decimal> CalculateRevenueAmountAsync(
        CancellationToken cancellationToken,
        int? softwareId = null,
        string? currency = null
    );

    public Task<decimal> CalculatePredictedRevenueAmountAsync(
        CancellationToken cancellationToken,
        int? softwareId = null,
        string? currency = null
    );
}