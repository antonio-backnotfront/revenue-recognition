namespace RevenueRecognition.Models.Subscription;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Client;
using Software;

[Table("Subscription")]
public class Subscription
{
    [Key] public int Id { get; set; }
    public int SoftwareId { get; set; }
    public int ClientId { get; set; }
    public int RenewalPeriodId { get; set; }
    public int SubscriptionStatusId { get; set; }
    [Column(TypeName = "decimal(12, 2)")] public decimal Price { get; set; }
    public DateTime RegisterDate { get; set; }

    [ForeignKey(nameof(SubscriptionStatusId))]
    public SubscriptionStatus SubscriptionStatus { get; set; }

    [ForeignKey(nameof(RenewalPeriodId))] public RenewalPeriod RenewalPeriod { get; set; }
    [ForeignKey(nameof(SoftwareId))] public Software Software { get; set; }
    [ForeignKey(nameof(ClientId))] public Client Client { get; set; }
    public ICollection<DiscountSubscription> DiscountSubscriptions { get; set; }
    public ICollection<SubscriptionPayment> SubscriptionPayments { get; set; }
}