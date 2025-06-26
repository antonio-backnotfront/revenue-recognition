namespace RevenueRecognition.Models.Subscription;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RevenueRecognition.Models.Contract;

[Table("SubscriptionPayment")]
public class SubscriptionPayment
{
    [Key] public int Id { get; set; }

    public int SubscriptionId { get; set; }
    [ForeignKey(nameof(SubscriptionId))] public Subscription Subscription { get; set; }

    [Column(TypeName = "decimal(12, 2)")] public decimal Amount { get; set; }

    public DateTime PaidAt { get; set; }
}