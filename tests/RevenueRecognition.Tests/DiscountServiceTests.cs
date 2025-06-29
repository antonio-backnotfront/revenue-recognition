using Xunit;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using RevenueRecognition.Application.Services.Contract;
using RevenueRecognition.Application.DTOs.Contract;
using RevenueRecognition.Application.DTOs.Payment;
using RevenueRecognition.Application.Exceptions;
using RevenueRecognition.Infrastructure.Repositories.Contract;
using RevenueRecognition.Infrastructure.Repositories.UnitOfWork;
using RevenueRecognition.Application.Services.Client;
using RevenueRecognition.Application.Services.Software;
using RevenueRecognition.Application.Services.Subscription;
using RevenueRecognition.Application.Services.Discount;
using RevenueRecognition.Models.Contract;
using RevenueRecognition.Models.Discount;
using RevenueRecognition.Models.Software;
using RevenueRecognition.Models.Subscription;
using System.Collections.Generic;
using RevenueRecognition.Application.DTOs.Client;

public class ContractServiceTests
{
    private readonly Mock<IContractRepository> _contractRepoMock = new();
    private readonly Mock<IDiscountService> _discountServiceMock = new();
    private readonly Mock<IClientService> _clientServiceMock = new();
    private readonly Mock<ISubscriptionService> _subscriptionServiceMock = new();
    private readonly Mock<ISoftwareService> _softwareServiceMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly ContractService _service;

    public ContractServiceTests()
    {
        _service = new ContractService(
            _contractRepoMock.Object,
            _discountServiceMock.Object,
            _clientServiceMock.Object,
            _subscriptionServiceMock.Object,
            _softwareServiceMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task CreateContractOrThrowAsync_ValidRequest_ReturnsContractResponse()
    {
        // Arrange
        var request = new CreateContractRequest
        {
            ClientId = 1,
            SoftwareVersionId = 2,
            StartDate = DateTime.Now.AddDays(5),
            EndDate = DateTime.Now.AddDays(10),
            YearsOfSupport = 2
        };

        var softwareVersion = new SoftwareVersion { Id = 2, SoftwareId = 3, Software = new Software { Cost = 1000 } };
        var clientDto = new GetClientResponse { Id = 1, IsLoyal = true };

        _clientServiceMock.Setup(s => s.GetClientByIdOrThrowAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(clientDto);
        _softwareServiceMock
            .Setup(s => s.GetSoftwareVersionBySoftwareVersionIdOrThrowAsync(2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(softwareVersion);
        _contractRepoMock
            .Setup(r => r.GetContractsByClientIdAndSoftwareVersionIdAsync(1, 3, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Contract>());
        _subscriptionServiceMock.Setup(s => s.GetSubscriptionsByClientIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Subscription>());
        _discountServiceMock.Setup(s => s.GetDiscountByDateAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Discount)null);
        _discountServiceMock.Setup(s => s.GetLoyalClientDiscountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((Discount)null);
        _contractRepoMock.Setup(r => r.GetContractStatusIdByNameAsync("Active", It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _contractRepoMock.Setup(r => r.InsertContractAsync(It.IsAny<Contract>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Contract c, CancellationToken ct) =>
            {
                c.Id = 100;
                return c;
            });

        // Act
        var result = await _service.CreateContractOrThrowAsync(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.ClientId);
        Assert.Equal(2, result.SoftwareVersionId);
        Assert.Equal(100, result.Id);
    }

    [Fact]
    public async Task CreateContractOrThrowAsync_InvalidDates_ThrowsBadRequest()
    {
        // Arrange
        var request = new CreateContractRequest
        {
            ClientId = 1,
            SoftwareVersionId = 2,
            StartDate = DateTime.Now.AddDays(11),
            EndDate = DateTime.Now.AddDays(10),
            YearsOfSupport = 2
        };

        var clientDto = new GetClientResponse { Id = 1, IsLoyal = false };
        var softwareVersion = new SoftwareVersion { Id = 2, SoftwareId = 3, Software = new Software { Cost = 1000 } };
        _contractRepoMock
            .Setup(r => r.GetContractsByClientIdAndSoftwareVersionIdAsync(It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Contract>()); // Return empty list, NOT null

        _subscriptionServiceMock
            .Setup(s => s.GetSubscriptionsByClientIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Subscription>()); // Return empty list, NOT null

        _clientServiceMock.Setup(s => s.GetClientByIdOrThrowAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(clientDto);
        _softwareServiceMock
            .Setup(s => s.GetSoftwareVersionBySoftwareVersionIdOrThrowAsync(2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(softwareVersion);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            _service.CreateContractOrThrowAsync(request, CancellationToken.None));
    }

    [Fact]
    public async Task IssuePaymentByIdOrThrowAsync_ValidPartialPayment_CommitsTransaction()
    {
        // Arrange
        var contract = new Contract
        {
            Id = 1, ContractStatusId = 2, Paid = 100, Price = 300,
            ContractStatus = new ContractStatus { Name = "Active" }, ClientId = 10
        };

        _contractRepoMock.Setup(r => r.GetContractByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(contract);
        _contractRepoMock.Setup(r => r.GetContractStatusIdByNameAsync("Active", It.IsAny<CancellationToken>()))
            .ReturnsAsync(2);
        _contractRepoMock.Setup(r => r.InsertPaymentAsync(It.IsAny<ContractPayment>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ContractPayment { Id = 123, ContractId = 1, Amount = 200, PaidAt = DateTime.Now });
        _contractRepoMock
            .Setup(r => r.GetContractStatusIdByNameAsync("Paid", It.IsAny<CancellationToken>()))
            .ReturnsAsync(3);

        var request = new CreatePaymentRequest { Amount = 200 };

        var result = await _service.IssuePaymentByIdOrThrowAsync(1, request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(1, result.ContractId);
        Assert.Equal(200, result.Amount);
    }

    [Fact]
    public async Task DeleteContractByIdOrThrowAsync_DeletesAndCommits()
    {
        // Act
        await _service.DeleteContractByIdOrThrowAsync(5, CancellationToken.None);

        // Assert
        _contractRepoMock.Verify(r => r.DeleteContractByIdAsync(5, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}