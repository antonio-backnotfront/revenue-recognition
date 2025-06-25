namespace RevenueRecognition.Domain.Entities.Subscription;

public class Subscription
{
    public int Id { get; set; }
    public int SoftwareId { get; set; }
    public int ClientId { get; set; }
    public int RenewalPeriodId { get; set; }
    public decimal Price { get; set; }
    public string Name { get; set; }
}