using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.Application.DTOs.Payment;

public class CreatePaymentRequest
{
    [Required] public required decimal Amount { get; set; }
}