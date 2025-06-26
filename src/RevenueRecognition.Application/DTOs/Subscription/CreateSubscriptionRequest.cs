using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.Application.DTOs.Subscription;

public class CreateSubscriptionRequest
{
    [Required] public int SoftwareId { get; set; }
    [Required] public int ClientId { get; set; }
    [Required] public int RenewalPeriodId { get; set; }
}