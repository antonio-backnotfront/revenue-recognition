using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognition.Models.Contract;

[Table("Contract")]
public class Contract
{
    [Key]
    public int Id { get; set; }
    public int SoftwareVersionId { get; set; }
    public int ClientId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int YearsOfSupport { get; set; }
    [Column(TypeName = "decimal(12, 2)")]
    public decimal Price { get; set; }
    [Column(TypeName = "decimal(12, 2)")]
    public decimal Paid { get; set; }
    public DateTime SignedDate { get; set; }
    
    [ForeignKey(nameof(SoftwareVersionId))]
    public Software.SoftwareVersion SoftwareVersion { get; set; }
    [ForeignKey(nameof(ClientId))]
    public Client.Client Client { get; set; }
    public ICollection<ContractUpdateType> ContractUpdateTypes { get; set; }
    public ICollection<DiscountContract> DiscountContracts { get; set; }
}