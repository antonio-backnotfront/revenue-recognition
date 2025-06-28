using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.Application.DTOs.Contract;

public class CreateContractResponse
{
    public required int Id { get; set; }
    public required int SoftwareVersionId { get; set; }
    public required int ClientId { get; set; }
    public required int YearsOfSupport { get; set; } = 1;
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
}