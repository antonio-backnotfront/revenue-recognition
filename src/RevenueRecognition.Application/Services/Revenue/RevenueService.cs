namespace RevenueRecognition.Application.Services.Revenue;

public class RevenueService : IRevenueService
{
    public Task<decimal> CalculateRevenueAmountAsync(
        CancellationToken cancellationToken,
        int? softwareId = null,
        string? currency = null
    )
    {
        throw new NotImplementedException();
    }

    public Task<decimal> CalculatePredictedRevenueAmountAsync(
        CancellationToken cancellationToken,
        int? softwareId = null,
        string? currency = null
    )
    {
        throw new NotImplementedException();
    }
}