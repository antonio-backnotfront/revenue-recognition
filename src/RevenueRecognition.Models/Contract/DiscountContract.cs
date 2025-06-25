using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RevenueRecognition.Models.Contract;

[Table("DiscountContract")]
[PrimaryKey(nameof(DiscountId), nameof(ContractId))]
public class DiscountContract
{
    public int DiscountId { get; set; }
    public int ContractId { get; set; }

    [ForeignKey(nameof(ContractId))] public Contract Contract { get; set; }
    [ForeignKey(nameof(DiscountId))] public Discount.Discount Discount { get; set; }
}