namespace RevenueRecognition.Models.Contract;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Software;
using Client;

[Table("Contract")]
public class Contract
{
    [Key] public int Id { get; set; }
    public int SoftwareVersionId { get; set; }
    public int ClientId { get; set; }
    public int ContractStatusId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int YearsOfSupport { get; set; }
    [Column(TypeName = "decimal(12, 2)")] public decimal Price { get; set; }
    [Column(TypeName = "decimal(12, 2)")] public decimal Paid { get; set; }

    [ForeignKey(nameof(ContractStatusId))] public ContractStatus ContractStatus { get; set; }

    [ForeignKey(nameof(SoftwareVersionId))]
    public SoftwareVersion SoftwareVersion { get; set; }

    [ForeignKey(nameof(ClientId))] public Client Client { get; set; }
    public ICollection<ContractUpdateType> ContractUpdateTypes { get; set; }
    public ICollection<DiscountContract> DiscountContracts { get; set; }
    public ICollection<ContractPayment> ContractPayments { get; set; }
}