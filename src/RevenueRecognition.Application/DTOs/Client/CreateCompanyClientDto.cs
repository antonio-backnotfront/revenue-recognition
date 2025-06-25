using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.Application.DTOs.Client;

public class CreateCompanyClientDto
{
    [Required] [MaxLength(255)] public string Address { get; set; }
    [Required] [MaxLength(255)] [EmailAddress] public string Email { get; set; }
    [Required] [MaxLength(255)] [Phone] public string PhoneNumber { get; set; }
    [Required] [MaxLength(10)] public string KRS { get; set; }
    [Required] [MaxLength(64)] public string Name { get; set; }
}