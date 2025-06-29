using Moq;
using RevenueRecognition.Application.Services.Revenue;
using RevenueRecognition.Infrastructure.Repositories.Contract;
using RevenueRecognition.Infrastructure.Repositories.Subscription;
using RevenueRecognition.Models.Contract;
using RevenueRecognition.Models.Subscription;
using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RevenueRecognition.Application.Services.Currency;

public class RevenueServiceTests
{
    private readonly Mock<ICurrencyConverterService> _currencyConverterServiceMock = new();
    private readonly Mock<IContractRepository> _contractRepositoryMock = new();
    private readonly Mock<ISubscriptionRepository> _subscriptionRepositoryMock = new();

    private readonly RevenueService _service;

    public RevenueServiceTests()
    {
        _service = new RevenueService(
            _currencyConverterServiceMock.Object,
            _contractRepositoryMock.Object,
            _subscriptionRepositoryMock.Object
        );
    }

    [Fact]
    public async Task CalculateRevenueAmountAsync_WithCurrencyConversion_ReturnsConvertedTotal()
    {
        // Arrange
        var contracts = new List<Contract>
        {
            new Contract { Price = 100m, ContractStatus = new ContractStatus { Name = "Paid" } },
            new Contract { Price = 200m, ContractStatus = new ContractStatus { Name = "Active" } } // ignored
        };

        var payments = new List<SubscriptionPayment>
        {
            new SubscriptionPayment { Amount = 50m },
            new SubscriptionPayment { Amount = 75m }
        };

        _contractRepositoryMock
            .Setup(r => r.GetAllContractsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(contracts);

        _subscriptionRepositoryMock
            .Setup(r => r.GetAllPaymentsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(payments);

        _currencyConverterServiceMock
            .Setup(c => c.ConvertAsync("USD", 225m, It.IsAny<CancellationToken>()))
            .ReturnsAsync(180m);

        // Act
        var result = await _service.CalculateRevenueAmountAsync(CancellationToken.None, null, "USD");

        // Assert
        // 100 (Paid contract) + 50 + 75 = 225, converted to 180
        Assert.Equal(180m, result);

        _currencyConverterServiceMock.Verify(c => c.ConvertAsync("USD", 225m, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CalculateRevenueAmountAsync_WithoutCurrency_ReturnsTotal()
    {
        // Arrange
        var contracts = new List<Contract>
        {
            new Contract { Price = 100m, ContractStatus = new ContractStatus { Name = "Paid" } },
            new Contract { Price = 200m, ContractStatus = new ContractStatus { Name = "Active" } } // ignored
        };

        var payments = new List<SubscriptionPayment>
        {
            new SubscriptionPayment { Amount = 50m },
            new SubscriptionPayment { Amount = 75m }
        };

        _contractRepositoryMock
            .Setup(r => r.GetAllContractsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(contracts);

        _subscriptionRepositoryMock
            .Setup(r => r.GetAllPaymentsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(payments);

        // Act
        var result = await _service.CalculateRevenueAmountAsync(CancellationToken.None);

        // Assert
        // 100 (Paid contract) + 50 + 75 = 225
        Assert.Equal(225m, result);

        // Conversion service should not be called if currency is null or empty
        _currencyConverterServiceMock.Verify(
            c => c.ConvertAsync(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}