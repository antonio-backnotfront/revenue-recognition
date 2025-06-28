namespace RevenueRecognition.Application.Services.Client;

using RevenueRecognition.Application.DTOs.Client;
using Models.Client;

public interface IClientService
{
    public Task<List<GetClientResponse>> GetClientsAsync(
        CancellationToken cancellationToken
    );

    public Task<GetClientResponse> CreateClientOrThrowAsync(
        CreateClientRequest request,
        CancellationToken cancellationToken
    );

    public Task<bool> RemoveClientByIdOrThrowAsync(
        int id,
        CancellationToken cancellationToken
    );

    public Task<GetClientResponse> UpdateClientByIdOrThrowAsync(
        int id,
        UpdateClientDto dto,
        CancellationToken cancellationToken
    );

    public Task<GetClientResponse> GetClientByIdOrThrowAsync(
        int id,
        CancellationToken cancellationToken
    );

    public Task<bool> IsClientLoyalByIdOrThrowAsync(
        int id,
        CancellationToken cancellationToken
    );
    
    public Task<bool> SetIsClientLoyalByIdOrThrowAsync(
        int clientId,
        bool isLoyal,
        CancellationToken cancellationToken
    );
}