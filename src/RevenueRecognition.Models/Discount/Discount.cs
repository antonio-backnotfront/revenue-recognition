using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RevenueRecognition.Models.Contract;
using RevenueRecognition.Models.Subscription;

namespace RevenueRecognition.Models.Discount;

[Table("Discount")]
public class Discount
{
    [Key] public int Id { get; set; }

    [Column(TypeName = "decimal(4, 2)")] public decimal Value { get; set; }
    [MaxLength(255)] public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public ICollection<DiscountContract> DiscountContracts { get; set; }
    public ICollection<DiscountSubscription> DiscountSubscriptions { get; set; }
}