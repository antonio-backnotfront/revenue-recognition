namespace RevenueRecognition.Application.Services.Client;

using RevenueRecognition.Application.DTOs.Client;
using Models.Client;

public interface IClientService
{
    public Task<List<GetClientResponse>> GetClientsAsync(
        CancellationToken cancellationToken
    );

    public Task<GetClientResponse> CreateClientAsync(
        CreateClientRequest request,
        CancellationToken cancellationToken
    );

    public Task<bool> RemoveClientAsync(
        int id,
        CancellationToken cancellationToken
    );

    public Task<GetClientResponse> UpdateClientAsync(
        int id,
        UpdateClientDto dto,
        CancellationToken cancellationToken
    );

    public Task<GetClientResponse> GetClientByIdAsync(
        int id,
        CancellationToken cancellationToken
    );

    public Task<bool> IsClientLoyalById(
        int id,
        CancellationToken cancellationToken
    );
    
    public Task<bool> SetIsClientLoyalById(
        int clientId,
        bool isLoyal,
        CancellationToken cancellationToken
    );
}