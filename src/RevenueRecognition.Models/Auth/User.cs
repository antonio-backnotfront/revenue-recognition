using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognition.Models.Auth;

[Table("User")]
public class User
{
    [Key]
    public int Id { get; set; }
    [MaxLength(255)]
    public string Login { get; set; }
    [MaxLength(255)]
    public string Password { get; set; }
    
    public int RoleId { get; set; }
    [ForeignKey(nameof(RoleId))]
    public Role Role { get; set; }
}