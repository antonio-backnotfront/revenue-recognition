namespace RevenueRecognition.Application.DTOs.Client;

public class GetCompanyClientDto
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string Name { get; set; }
    public string KRS { get; set; }
}