namespace RevenueRecognition.Application.DTOs.Payment;

public class CreateContractPaymentResponse
{
    public required int Id { get; set; }
    public required int ContractId { get; set; }
    public required decimal Amount { get; set; }
    public required DateTime DateOfPayment { get; set; }
}