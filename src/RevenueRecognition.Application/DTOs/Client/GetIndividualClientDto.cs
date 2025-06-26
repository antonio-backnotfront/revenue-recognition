namespace RevenueRecognition.Application.DTOs.Client;

public class GetIndividualClientDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PESEL { get; set; }
}