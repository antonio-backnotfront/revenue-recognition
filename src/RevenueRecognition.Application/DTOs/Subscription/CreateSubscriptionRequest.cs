using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.Application.DTOs.Subscription;

public class CreateSubscriptionRequest
{
    [Required] public required int SoftwareId { get; set; }
    [Required] public required int ClientId { get; set; }
    [Required] public required int RenewalPeriodId { get; set; }
}