namespace RevenueRecognition.Application.Services.Token;

public interface ITokenService
{
    string GenerateToken(string username, string role);
}