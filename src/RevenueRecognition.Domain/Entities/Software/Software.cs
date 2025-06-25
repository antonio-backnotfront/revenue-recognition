namespace RevenueRecognition.Domain.Entities.Software;

public class Software
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public int CurrentVersionId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}