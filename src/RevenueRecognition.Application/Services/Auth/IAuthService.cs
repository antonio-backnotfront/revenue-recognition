using RevenueRecognition.Application.DTOs.Auth;

namespace RevenueRecognition.Application.Services.Auth;

public interface IAuthService
{
    public Task<AccountLoginResponse?> ValidateLoginByUsernameAndPassword(AccountLoginRequest request,
        CancellationToken cancellationToken);
}