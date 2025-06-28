namespace RevenueRecognition.Application.Services.Software;

using RevenueRecognition.Models.Software;

public interface ISoftwareService
{
    public Task<SoftwareVersion> GetSoftwareVersionBySoftwareVersionIdOrThrowAsync(
        int id,
        CancellationToken cancellationToken
    );

    public Task<Software> GetSoftwareByIdOrThrowAsync(
        int id,
        CancellationToken cancellationToken
    );
}