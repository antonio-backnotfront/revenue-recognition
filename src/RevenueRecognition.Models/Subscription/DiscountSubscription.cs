using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RevenueRecognition.Models.Subscription;

[Table("DiscountSubscription")]
[PrimaryKey(nameof(SubscriptionId),nameof(DiscountId))]
public class DiscountSubscription
{
    public int SubscriptionId { get; set; }
    public int DiscountId { get; set; }
    
    [ForeignKey(nameof(SubscriptionId))]
    public Subscription Subscription { get; set; }
    [ForeignKey(nameof(DiscountId))]
    public Discount.Discount Discount { get; set; }
}