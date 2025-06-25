using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace RevenueRecognition.Models.Client;
[Table("IndividualClient")]
public class IndividualClient
{
    [Key]
    public int Id { get; set; }
    public int ClientId { get; set; }
    [MaxLength(64)]
    public string? FirstName { get; set; }
    [MaxLength(64)]
    public string? LastName { get; set; }
    [MaxLength(10)]
    public string? PESEL { get; set; }
    public bool IsDeleted { get; set; }
    [ForeignKey(nameof(ClientId))]
    public Client Client { get; set; }
}