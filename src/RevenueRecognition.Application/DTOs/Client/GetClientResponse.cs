namespace RevenueRecognition.Application.DTOs.Client;

public class GetClientResponse
{
    public int Id { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsLoyal { get; set; }
    public GetCompanyClientDto? CompanyInformation { get; set; }
    public GetIndividualClientDto? IndividualInformation { get; set; }
}