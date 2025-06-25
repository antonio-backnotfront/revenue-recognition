namespace RevenueRecognition.Domain.Entities.Software;

public class Version
{
    public int Id { get; set; }
    public int SoftwareId { get; set; }
    public string VersionNumber { get; set; }
    public string Description { get; set; }
    public DateTime ReleaseDate { get; set; }
}