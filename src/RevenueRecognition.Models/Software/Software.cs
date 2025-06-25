using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognition.Models.Software;

[Table("Software")]
public class Software
{
    [Key]
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public int CurrentVersionId { get; set; }
    [MaxLength(255)]
    public string Name { get; set; }
    [MaxLength(255)]
    public string Description { get; set; }
    [Column(TypeName = "decimal(12, 2)")]
    public decimal Cost { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; }
    [ForeignKey(nameof(CurrentVersionId))]
    [NotMapped]
    public SoftwareVersion CurrentSoftwareVersion { get; set; }
    public ICollection<SoftwareVersion> Versions { get; set; }
    public ICollection<Subscription.Subscription> Subscriptions { get; set; }
}