using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RevenueRecognition.Application.DTOs.Client;

public class CreateClientRequest
{
    public int? Id { get; set; }
    [Required] [MaxLength(255)] public string Address { get; set; }
    [Required] [MaxLength(255)] [EmailAddress] public string Email { get; set; }
    [Required] [Phone] public string PhoneNumber { get; set; }
    public CreateCompanyClientDto? CompanyInformation { get; set; }
    public CreateIndividualClientDto? IndividualInformation { get; set; }
}