using RevenueRecognition.Application.Exceptions;
using RevenueRecognition.Infrastructure.Repositories.Software;
using RevenueRecognition.Models.Software;

namespace RevenueRecognition.Application.Services.Software;

public class SoftwareService : ISoftwareService
{
    private readonly ISoftwareRepository _repository;

    public SoftwareService(ISoftwareRepository repository)
    {
        _repository = repository;
    }

    public async Task<SoftwareVersion> GetSoftwareVersionBySoftwareVersionIdAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        SoftwareVersion? softwareVersion = await _repository
            .GetSoftwareVersionBySoftwareVersionIdAsync(id, cancellationToken);
        if (softwareVersion == null)
            throw new NotFoundException($"SoftwareVersion with id {id} not found.");
        return softwareVersion;
    }
}