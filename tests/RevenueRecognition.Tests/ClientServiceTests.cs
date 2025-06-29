// Required using directives

using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RevenueRecognition.Application.Services.Client;
using RevenueRecognition.Application.DTOs.Client;
using RevenueRecognition.Infrastructure.Repositories.Client;
using RevenueRecognition.Infrastructure.Repositories.UnitOfWork;
using RevenueRecognition.Infrastructure.Repositories.Discount;
using RevenueRecognition.Models.Client;
using RevenueRecognition.Application.Exceptions;

public class ClientServiceTests
{
    private readonly Mock<IClientRepository> _clientRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IDiscountRepository> _discountRepoMock;
    private readonly ClientService _service;

    public ClientServiceTests()
    {
        _clientRepoMock = new Mock<IClientRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _discountRepoMock = new Mock<IDiscountRepository>();
        _service = new ClientService(_clientRepoMock.Object, _unitOfWorkMock.Object, _discountRepoMock.Object);
    }

    [Fact]
    public async Task GetClientByIdOrThrowAsync_ClientExists_ReturnsClientDto()
    {
        var client = new Client { Id = 1, Email = "mail@mail.com", PhoneNumber = "123", IsLoyal = true };
        var individual = new IndividualClient { Id = 1, PESEL = "999", FirstName = "John", LastName = "Doe" };

        _clientRepoMock.Setup(r => r.GetClientByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(client);
        _clientRepoMock.Setup(r => r.GetCompanyClientByClientIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CompanyClient)null);
        _clientRepoMock.Setup(r => r.GetIndividualClientByClientIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(individual);

        var result = await _service.GetClientByIdOrThrowAsync(1, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("mail@mail.com", result.Email);
        Assert.Equal("John", result.IndividualInformation.FirstName);
    }

    [Fact]
    public async Task GetClientByIdOrThrowAsync_DeletedIndividual_ThrowsNotFound()
    {
        var client = new Client { Id = 1 };
        var individual = new IndividualClient { Id = 1, IsDeleted = true };

        _clientRepoMock.Setup(r => r.GetClientByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(client);
        _clientRepoMock.Setup(r => r.GetCompanyClientByClientIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CompanyClient)null);
        _clientRepoMock.Setup(r => r.GetIndividualClientByClientIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(individual);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.GetClientByIdOrThrowAsync(1, CancellationToken.None));
    }

    [Fact]
    public async Task CreateClientOrThrowAsync_BothClientTypes_ThrowsBadRequest()
    {
        var request = new CreateClientRequest
        {
            Email = "client@mail.com",
            PhoneNumber = "123",
            CompanyInformation = new CreateCompanyClientDto { KRS = "123", Name = "ACME" },
            IndividualInformation = new CreateIndividualClientDto
                { PESEL = "999", FirstName = "John", LastName = "Doe" }
        };

        await Assert.ThrowsAsync<BadRequestException>(() =>
            _service.CreateClientOrThrowAsync(request, CancellationToken.None));
    }

    [Fact]
    public async Task SetIsClientLoyalByIdOrThrowAsync_ClientExists_UpdatesAndReturnsTrue()
    {
        var client = new Client { Id = 2, IsLoyal = false };

        _clientRepoMock.Setup(r => r.GetClientByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(client);
        _clientRepoMock.Setup(r => r.SetIsLoyalAsync(client, true, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await _service.SetIsClientLoyalByIdOrThrowAsync(2, true, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task RemoveClientByIdOrThrowAsync_CompanyClient_ThrowsForbid()
    {
        var client = new Client { Id = 5, CompanyClient = new CompanyClient() };

        _clientRepoMock.Setup(r => r.GetClientByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(client);

        await Assert.ThrowsAsync<ForbidException>(
            () => _service.RemoveClientByIdOrThrowAsync(5, CancellationToken.None));
    }

    [Fact]
    public async Task RemoveClientByIdOrThrowAsync_ValidIndividual_SoftDeletesAndReturnsTrue()
    {
        var client = new Client { Id = 3 };

        _clientRepoMock.Setup(r => r.GetClientByIdAsync(3, It.IsAny<CancellationToken>())).ReturnsAsync(client);
        _clientRepoMock.Setup(r => r.IsDeletedByClientId(3, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var result = await _service.RemoveClientByIdOrThrowAsync(3, CancellationToken.None);

        Assert.True(result);
        _clientRepoMock.Verify(r => r.SoftDeleteByClientId(3, It.IsAny<CancellationToken>()), Times.Once);
    }
}