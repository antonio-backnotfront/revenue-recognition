using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.Application.DTOs.Contract;

public class CreateContractDto
{
    [Required] public int SoftwareVersionId { get; set; }
    [Required] public int ClientId { get; set; }
    [Required] public int YearsOfSupport { get; set; }
    [Required] public DateTime StartDate { get; set; }
    [Required] public DateTime EndDate { get; set; }
}