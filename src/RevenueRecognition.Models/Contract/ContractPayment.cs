namespace RevenueRecognition.Models.Contract;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("ContractPayment")]
public class ContractPayment
{
    [Key] public int Id { get; set; }

    public int ContractId { get; set; }
    [ForeignKey(nameof(ContractId))] public Contract Contract { get; set; }

    [Column(TypeName = "decimal(12, 2)")] public decimal Amount { get; set; }

    public DateTime PaidAt { get; set; }
}