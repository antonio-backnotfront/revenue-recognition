namespace RevenueRecognition.Models.Contract;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table("UpdateType")]
public class UpdateType
{
    [Key] public int Id { get; set; }
    [MaxLength(255)] public string Name { get; set; }

    public ICollection<ContractUpdateType> ContractUpdateTypes { get; set; }
}