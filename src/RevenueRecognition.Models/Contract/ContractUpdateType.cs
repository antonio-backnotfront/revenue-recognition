namespace RevenueRecognition.Models.Contract;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

[Table("ContractUpdateType")]
[PrimaryKey(nameof(UpdateTypeId), nameof(ContractId))]
public class ContractUpdateType
{
    public int UpdateTypeId { get; set; }
    public int ContractId { get; set; }
    [ForeignKey(nameof(UpdateTypeId))] public UpdateType UpdateType { get; set; }
    [ForeignKey(nameof(ContractId))] public Contract Contract { get; set; }
}