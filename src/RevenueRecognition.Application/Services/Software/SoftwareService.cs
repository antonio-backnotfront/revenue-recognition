namespace RevenueRecognition.Application.Services.Software;

using Exceptions;
using RevenueRecognition.Infrastructure.Repositories.Software;
using RevenueRecognition.Models.Software;

public class SoftwareService : ISoftwareService
{
    private readonly ISoftwareRepository _repository;

    public SoftwareService(ISoftwareRepository repository)
    {
        _repository = repository;
    }

    public async Task<SoftwareVersion> GetSoftwareVersionBySoftwareVersionIdOrThrowAsync(
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

    public async Task<Software> GetSoftwareByIdOrThrowAsync(int id, CancellationToken cancellationToken)
    {
        Software? software = await _repository.GetSoftwareByIdAsync(id, cancellationToken);
        if (software == null)
            throw new NotFoundException($"Software with id {id} not found.");
        return software;
    }
}