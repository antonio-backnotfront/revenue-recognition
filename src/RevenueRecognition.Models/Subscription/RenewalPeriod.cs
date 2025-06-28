using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognition.Models.Subscription;

[Table("RenewalPeriod")]
public class RenewalPeriod
{
    [Key] public int Id { get; set; }
    [MaxLength(64)] public string Name { get; set; }
    [Range(30,730)] public int Days { get; set; }

    public ICollection<Subscription> Subscriptions { get; set; }
}