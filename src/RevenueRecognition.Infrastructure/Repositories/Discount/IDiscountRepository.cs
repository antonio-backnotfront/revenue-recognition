namespace RevenueRecognition.Infrastructure.Repositories.Discount;

using Models.Discount;

public interface IDiscountRepository
{
    public Task<Discount?> GetHighestDiscountByDateAsync(DateTime date, CancellationToken cancellationToken);
}