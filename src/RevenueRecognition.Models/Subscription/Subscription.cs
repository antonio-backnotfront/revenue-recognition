using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognition.Models.Subscription;

[Table("Subscription")]
public class Subscription
{
    [Key]
    public int Id { get; set; }
    public int SoftwareId { get; set; }
    public int ClientId { get; set; }
    public int RenewalPeriodId { get; set; }
    [Column(TypeName = "decimal(12, 2)")]
    public decimal Price { get; set; }
    public DateTime RegisterDate { get; set; }

    [ForeignKey(nameof(RenewalPeriodId))]
    public RenewalPeriod RenewalPeriod { get; set; }
    [ForeignKey(nameof(SoftwareId))]
    public Software.Software Software { get; set; }
    [ForeignKey(nameof(ClientId))]
    public Client.Client Client { get; set; }
    public ICollection<DiscountSubscription> DiscountSubscriptions { get; set; }
}