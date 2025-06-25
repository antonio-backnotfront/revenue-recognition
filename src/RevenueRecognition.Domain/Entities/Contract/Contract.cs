namespace RevenueRecognition.Domain.Entities.Contract;

public class Contract
{
    public int Id { get; set; }
    public int SoftwareVersionId { get; set; }
    public int ClientId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int YearsOfSupport { get; set; }
    public decimal Price { get; set; }
    public decimal Paid { get; set; }
    public DateTime SignedDate { get; set; }
}