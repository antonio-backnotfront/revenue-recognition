namespace RevenueRecognition.Infrastructure.Repositories.Software;

using Models.Software;

public interface ISoftwareRepository
{
    public Task<SoftwareVersion?> GetSoftwareVersionBySoftwareVersionIdAsync(
        int id,
        CancellationToken cancellationToken
    );
}