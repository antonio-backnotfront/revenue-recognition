using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.Application.DTOs.Payment;

public class CreatePaymentDto
{
    [Required] public decimal Amount { get; set; }
}