namespace RevenueRecognition.Domain.Entities.Client;

public class IndividualClient
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PESEL { get; set; }
    public bool IsDeleted { get; set; }
}