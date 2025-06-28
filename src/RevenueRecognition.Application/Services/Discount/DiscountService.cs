using RevenueRecognition.Infrastructure.Repositories.Discount;

namespace RevenueRecognition.Application.Services.Discount;

using Models.Discount;

public class DiscountService : IDiscountService
{
    IDiscountRepository _repository;

    public DiscountService(IDiscountRepository repository)
    {
        _repository = repository;
    }

    public async Task<Discount?> GetDiscountByDateAsync(DateTime date, CancellationToken cancellationToken)
    {
        return await _repository.GetHighestDiscountByDateAsync(date, cancellationToken);
    }

    public async Task<Discount?> GetLoyalClientDiscountAsync(CancellationToken cancellationToken)
    {
        return await _repository.GetLoyalClientDiscountAsync(cancellationToken);
    }
}