namespace RevenueRecognition.Models.Contract;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("ContractStatus")]
public class ContractStatus
{
    [Key] public int Id { get; set; }
    [MaxLength(64)] public string Name { get; set; }

    public ICollection<Contract> Contracts { get; set; }
}