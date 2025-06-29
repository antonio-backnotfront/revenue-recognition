using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace RevenueRecognition.Application.Services.Currency;

using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class CurrencyConverterService : ICurrencyConverterService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly string _apiKey;

    public CurrencyConverterService(HttpClient httpClient, IConfiguration configuration)
    {
        _configuration = configuration;
        _apiKey = configuration["CurrencyApi:key"];
        _httpClient = httpClient;
    }

    public async Task<decimal> ConvertAsync(string toCurrency, decimal amount, CancellationToken cancellationToken)
    {
        string url = $"https://api.exchangerate.host/convert?from=PLN&to={toCurrency}&amount={amount.ToString().Replace(",",".")}&access_key={_apiKey}";
        Console.WriteLine($"{url}");
        var response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        Console.WriteLine($"{json}");
        var data = JsonSerializer.Deserialize<ExchangeRateResponse>(json);
        Console.WriteLine($"{data?.result}");
        return data?.result ?? throw new Exception("Could not parse exchange rate.");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

public class ExchangeRateResponse
{
    public decimal result { get; set; }
}
