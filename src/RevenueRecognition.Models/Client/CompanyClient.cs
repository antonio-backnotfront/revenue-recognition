namespace RevenueRecognition.Models.Client;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("CompanyClient")]
public class CompanyClient
{
    [Key] public int Id { get; set; }
    public int ClientId { get; set; }
    [MaxLength(64)] public string Name { get; set; }
    [MaxLength(10)] public string KRS { get; set; }
    [ForeignKey(nameof(ClientId))] public Client Client { get; set; }
}