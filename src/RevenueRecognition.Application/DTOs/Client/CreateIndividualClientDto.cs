using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.Application.DTOs.Client;

public class CreateIndividualClientDto
{
    public int? Id { get; set; }
    [Required] [MaxLength(255)] public string Address { get; set; }
    [Required] [MaxLength(255)] [EmailAddress] public string Email { get; set; }
    [Required] [Phone] public string PhoneNumber { get; set; }
    [Required] [MaxLength(64)] public string FirstName { get; set; }
    [Required] [MaxLength(64)] public string LastName { get; set; }
    [Required] [MaxLength(11)] public string PESEL { get; set; }
}