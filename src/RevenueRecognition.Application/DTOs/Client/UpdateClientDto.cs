using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.Application.DTOs.Client;

public class UpdateClientDto
{
    
    // General Information
    [MaxLength(255)] public string? Address { get; set; }
    [MaxLength(255)] [EmailAddress] public string? Email { get; set; }
    [MaxLength(255)] [Phone] public string? PhoneNumber { get; set; }
    
    // IndividualClient Information
    [MaxLength(64)] public string? FirstName { get; set; }
    [MaxLength(64)] public string? LastName { get; set; }
    [MaxLength(11)] public string? PESEL { get; set; }
    
    // CompanyClient Information
    [MaxLength(64)] public string? Name { get; set; }
    [MaxLength(10)] public string? KRS { get; set; }
}