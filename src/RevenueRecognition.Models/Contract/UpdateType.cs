using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.Models.Contract;

public class UpdateType
{
    [Key]
    public int Id { get; set; }
    [MaxLength(255)]
    public string Name { get; set; }
    
    public ICollection<ContractUpdateType> ContractUpdateTypes { get; set; }
}