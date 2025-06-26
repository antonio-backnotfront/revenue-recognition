using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using RevenueRecognition.Application.Enums;

namespace RevenueRecognition.Application.DTOs.Client;

public class CreateClientRequest
{
    public int? Id { get; set; }
    [Required]
    public string Type { get; set; }
    public CreateCompanyClientDto? CompanyInformation { get; set; }
    public CreateIndividualClientDto? IndividualInformation { get; set; }
}