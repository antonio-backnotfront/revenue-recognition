namespace RevenueRecognition.Models.Software;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Contract;

[Table("SoftwareVersion")]
public class SoftwareVersion
{
    [Key] public int Id { get; set; }
    public int SoftwareId { get; set; }
    [MaxLength(64)] public string VersionNumber { get; set; }
    [MaxLength(255)] public string Description { get; set; }
    public DateTime ReleaseDate { get; set; }

    [ForeignKey(nameof(SoftwareId))] public Software Software { get; set; }
    public ICollection<Contract> Contracts { get; set; }
}