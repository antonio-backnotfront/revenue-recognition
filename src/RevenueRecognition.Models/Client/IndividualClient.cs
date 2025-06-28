namespace RevenueRecognition.Models.Client;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("IndividualClient")]
public class IndividualClient
{
    [Key] public int Id { get; set; }
    public int ClientId { get; set; }
    [MaxLength(64)] public string FirstName { get; set; }
    [MaxLength(64)] public string LastName { get; set; }
    [MaxLength(11)] public string PESEL { get; set; }
    public bool IsDeleted { get; set; } = false;
    [ForeignKey(nameof(ClientId))] public Client Client { get; set; }
}