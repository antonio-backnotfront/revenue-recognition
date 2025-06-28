namespace RevenueRecognition.Application.DTOs.Payment;

public class CreateSubscriptionPaymentResponse
{
    public required int Id { get; set; }
    public required int SubscriptionId { get; set; }
    public required decimal Amount { get; set; }
    public required DateTime DateOfPayment { get; set; }
}