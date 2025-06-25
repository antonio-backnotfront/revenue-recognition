using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognition.Models.Auth;

[Table("Role")]
public class Role
{
    [Key]
    public int Id { get; set; }
    [MaxLength(255)]
    public string Name { get; set; }
    public ICollection<User> Users { get; set; }
}