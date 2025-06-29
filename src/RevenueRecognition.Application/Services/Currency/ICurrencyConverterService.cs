using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace RevenueRecognition.Application.Services.Currency;

using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public interface ICurrencyConverterService
{
    public Task<decimal> ConvertAsync(string toCurrency, decimal amount, CancellationToken cancellationToken);
}