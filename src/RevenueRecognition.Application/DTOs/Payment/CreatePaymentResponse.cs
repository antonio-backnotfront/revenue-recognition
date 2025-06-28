using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.Application.DTOs.Payment;

public class CreatePaymentResponse
{
    public required int? Id { get; set; }
    public required int? ContractId { get; set; }
    public required decimal Amount { get; set; }
    public required DateTime? DateOfPayment { get; set; }
}