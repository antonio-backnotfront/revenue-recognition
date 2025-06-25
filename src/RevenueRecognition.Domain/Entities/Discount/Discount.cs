namespace RevenueRecognition.Domain.Entities.Discount;

public class Discount
{
    public int Id { get; set; }
    public decimal Value { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}