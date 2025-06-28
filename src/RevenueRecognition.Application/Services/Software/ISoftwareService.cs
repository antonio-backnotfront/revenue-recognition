using RevenueRecognition.Models.Software;

namespace RevenueRecognition.Application.Services.Software;

public interface ISoftwareService
{
    public Task<SoftwareVersion> GetSoftwareVersionBySoftwareVersionIdAsync(
        int id,
        CancellationToken cancellationToken
    );
}