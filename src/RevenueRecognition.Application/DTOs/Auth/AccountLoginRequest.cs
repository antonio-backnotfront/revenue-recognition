using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.Application.DTOs.Auth;

public class AccountLoginRequest
{
    [Required] public required string Username { get; set; }
    [Required] public required string Password { get; set; }
}