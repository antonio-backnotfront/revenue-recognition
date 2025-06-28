using Microsoft.EntityFrameworkCore;

namespace RevenueRecognition.Application.DTOs.Subscription;

public class CreateSubscriptionResponse
{
    public required int Id { get; set; }
    public required int SoftwareId { get; set; }
    public required int SubscriptionStatusId { get; set; }
    public required int ClientId { get; set; }
    public required int RenewalPeriodId { get; set; }
    public required decimal Price { get; set; } 
    public required DateTime RegisterDate { get; set; }
}