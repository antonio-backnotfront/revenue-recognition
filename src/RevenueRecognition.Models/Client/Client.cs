namespace RevenueRecognition.Models.Client;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Subscription;
using Contract;

[Table("Client")]
public class Client
{
    public int Id { get; set; }
    [MaxLength(255)] public string Address { get; set; }
    [MaxLength(255)] public string Email { get; set; }
    [MaxLength(255)] public string PhoneNumber { get; set; }
    public bool IsLoyal { get; set; }
    public ICollection<Subscription> Subscriptions { get; set; }
    public ICollection<Contract> Contracts { get; set; }
    public IndividualClient? IndividualClient { get; set; }
    public CompanyClient? CompanyClient { get; set; }
}