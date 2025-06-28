namespace RevenueRecognition.Application.Services.Discount;

using Models.Discount;

public interface IDiscountService
{
    public Task<Discount?> GetDiscountByDateAsync(DateTime date, CancellationToken cancellationToken);
    public Task<Discount?> GetLoyalClientDiscountAsync(CancellationToken cancellationToken);
}