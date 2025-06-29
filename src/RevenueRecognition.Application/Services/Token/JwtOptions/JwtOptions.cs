using System.ComponentModel.DataAnnotations;

namespace RevenueRecognition.Application.Services.Token.JwtOptions;

public class JwtOptions
{
    [Required] public required string Issuer { get; set; }
    [Required] public required string Audience { get; set; }
    [Required] public required string Key { get; set; }
    [Required] public required int ValidInMinutes { get; set; }
}