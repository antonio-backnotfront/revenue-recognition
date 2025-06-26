using RevenueRecognition.Application.DTOs.Client;

namespace RevenueRecognition.Application.Services.Client;

public class ClientService : IClientService
{
    public Task<List<GetClientResponse>> GetClientsAsync(
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    public Task<CreateClientRequest> CreateClientAsync(
        CreateClientRequest request,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveClientAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    public Task<GetClientResponse> UpdateClientAsync(
        int id,
        UpdateClientDto dto,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    public Task<GetClientResponse> GetClientByIdAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}