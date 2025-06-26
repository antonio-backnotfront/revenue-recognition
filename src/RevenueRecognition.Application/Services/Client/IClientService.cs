using RevenueRecognition.Application.DTOs.Client;

namespace RevenueRecognition.Application.Services.Client;

public interface IClientService
{
    public Task<List<GetClientResponse>> GetClientsAsync(CancellationToken cancellationToken);
    public Task<CreateClientRequest> CreateClientAsync(CreateClientRequest request, CancellationToken cancellationToken);
    public Task<bool> RemoveClientAsync(int id, CancellationToken cancellationToken);
    public Task<GetClientResponse> UpdateClientAsync(int id, UpdateClientDto dto, CancellationToken cancellationToken);
    public Task<GetClientResponse> GetClientByIdAsync(int id, CancellationToken cancellationToken);
}