using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using RevenueRecognition.Application.Enums;

namespace RevenueRecognition.Application.DTOs.Client;

public class CreateClientRequest
{
    public int? Id { get; set; }
    [Required]
    public string Type { get; set; }
    public CreateCompanyClientDto? CompanyClient { get; set; }
    public CreateIndividualClientDto? IndividualClient { get; set; }
}