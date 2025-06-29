using Moq;
using RevenueRecognition.Application.DTOs.Client;
using RevenueRecognition.Application.DTOs.Payment;
using RevenueRecognition.Application.DTOs.Subscription;
using RevenueRecognition.Application.Exceptions;
using RevenueRecognition.Application.Services.Client;
using RevenueRecognition.Application.Services.Discount;
using RevenueRecognition.Application.Services.Software;
using RevenueRecognition.Application.Services.Subscription;
using RevenueRecognition.Infrastructure.Repositories.Subscription;
using RevenueRecognition.Infrastructure.Repositories.UnitOfWork;
using RevenueRecognition.Models.Discount;
using RevenueRecognition.Models.Software;
using RevenueRecognition.Models.Subscription;
using Xunit;

namespace RevenueRecognition.Tests;

public class SubscriptionServiceTests
{
    private readonly Mock<ISubscriptionRepository> _repoMock = new();
    private readonly Mock<ISoftwareService> _softwareMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IClientService> _clientMock = new();
    private readonly Mock<IDiscountService> _discountMock = new();
    private readonly SubscriptionService _service;

    public SubscriptionServiceTests()
    {
        _service = new SubscriptionService(
            _repoMock.Object,
            _softwareMock.Object,
            _unitOfWorkMock.Object,
            _clientMock.Object,
            _discountMock.Object);
    }

    [Fact]
    public async Task CreateSubscriptionOrThrowAsync_WhenClientHasActiveSubscription_ThrowsAlreadyExists()
    {
        //  Arrange
        var clientId = 1;
        var softwareId = 10;
        var renewalPeriodId = 5;

        _softwareMock.Setup(s => s.GetSoftwareByIdOrThrowAsync(softwareId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Software { Id = softwareId, Cost = 1200m });
        _clientMock.Setup(c => c.GetClientByIdOrThrowAsync(clientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetClientResponse { Id = clientId, IsLoyal = false });
        _repoMock.Setup(r => r.GetAllSubscriptionsByClientIdAsync(clientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Subscription>
            {
                new Subscription { SoftwareId = softwareId, SubscriptionStatusId = 1 }
            });
        _repoMock.Setup(r => r.GetSubscriptionStatusIdByNameAsync("Active", It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        _repoMock.Setup(r => r.GetRenewalPeriodByIdAsync(renewalPeriodId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new RenewalPeriod { Id = renewalPeriodId, Days = 30 });

        // act & Assert
        await Assert.ThrowsAsync<AlreadyExistsException>(() =>
            _service.CreateSubscriptionOrThrowAsync(
                new CreateSubscriptionRequest
                {
                    ClientId = clientId,
                    SoftwareId = softwareId,
                    RenewalPeriodId = renewalPeriodId
                }, CancellationToken.None));
    }

    [Fact]
    public async Task CreateSubscriptionOrThrowAsync_CreatesSubscriptionSuccessfully()
    {
        // Arrange
        var clientId = 1;
        var softwareId = 10;
        var renewalPeriodId = 5;

        _softwareMock.Setup(s => s.GetSoftwareByIdOrThrowAsync(softwareId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Software { Id = softwareId, Cost = 1200m });
        _clientMock.Setup(c => c.GetClientByIdOrThrowAsync(clientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetClientResponse { Id = clientId, IsLoyal = true });
        _repoMock.Setup(r => r.GetAllSubscriptionsByClientIdAsync(clientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Subscription>()); // no active subscriptions
        _repoMock.Setup(r => r.GetSubscriptionStatusIdByNameAsync("Active", It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        _repoMock.Setup(r => r.GetRenewalPeriodByIdAsync(renewalPeriodId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new RenewalPeriod { Id = renewalPeriodId, Days = 30 });
        _discountMock.Setup(d => d.GetDiscountByDateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Discount { Id = 100, Value = 10 }); // 10% discount
        _discountMock.Setup(d => d.GetLoyalClientDiscountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Discount { Id = 101, Value = 5 }); // 5% loyalty discount

        var createdSubscription = new Subscription
        {
            Id = 7,
            ClientId = clientId,
            SoftwareId = softwareId,
            RenewalPeriodId = renewalPeriodId,
            SubscriptionStatusId = 1,
            Price = 1026m,
            RegisterDate = DateTime.Now
        };

        _repoMock.Setup(r => r.InsertSubscriptionAsync(It.IsAny<Subscription>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdSubscription);
        _repoMock.Setup(r => r.InsertPaymentAsync(It.IsAny<SubscriptionPayment>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SubscriptionPayment { Id = 1, Amount = 100m, PaidAt = DateTime.Today });
        _repoMock.Setup(r =>
                r.InsertSubscriptionDiscountAsync(It.IsAny<DiscountSubscription>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DiscountSubscription());

        _clientMock.Setup(c => c.SetIsClientLoyalByIdOrThrowAsync(clientId, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _unitOfWorkMock.Setup(u => u.StartTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateSubscriptionOrThrowAsync(
            new CreateSubscriptionRequest
            {
                ClientId = clientId,
                SoftwareId = softwareId,
                RenewalPeriodId = renewalPeriodId
            }, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createdSubscription.Id, result.Id);
        Assert.Equal(createdSubscription.Price, result.Price);
        _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task IssuePaymentByIdOrThrowAsync_ThrowsIfSubscriptionNotActive()
    {
        // Arrange
        int subscriptionId = 10;
        _repoMock.Setup(r => r.GetSubscriptionByIdAsync(subscriptionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Subscription { SubscriptionStatusId = 2 });
        _repoMock.Setup(r => r.GetSubscriptionStatusIdByNameAsync("Active", It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act & assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            _service.IssuePaymentByIdOrThrowAsync(subscriptionId,
                new CreatePaymentRequest { Amount = 100m }, CancellationToken.None));
    }
}