using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognition.Domain.Entities.Client;

public class CompanyClient
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string Name { get; set; }
    public string KRS { get; set; }
}