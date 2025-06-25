using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.Application.DTOs.Client;

public class UpdateCompanyClientDto
{
    [MaxLength(255)] public string? Address { get; set; }
    [MaxLength(255)] [EmailAddress] public string? Email { get; set; }
    [MaxLength(255)] [Phone] public string? PhoneNumber { get; set; }
    [MaxLength(64)] public string? Name { get; set; }
}