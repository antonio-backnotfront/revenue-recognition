namespace RevenueRecognition.Infrastructure.Repositories.Software;

using Models.Software;

public interface ISoftwareRepository
{
    public Task<SoftwareVersion?> GetSoftwareVersionBySoftwareVersionIdAsync(
        int id,
        CancellationToken cancellationToken
    );

    public Task<Software?> GetSoftwareByIdAsync(
        int id,
        CancellationToken cancellationToken
    );
}