using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RevenueRecognition.Application.DTOs.Auth;
using RevenueRecognition.Infrastructure.DAL;
using RevenueRecognition.Models.Auth;

namespace RevenueRecognition.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly CompanyDbContext _context;

    public AuthService(CompanyDbContext context)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<AccountLoginResponse?> ValidateLoginByUsernameAndPassword(AccountLoginRequest request,
        CancellationToken cancellationToken)
    {
        var account = await _context.Users
            .Include(acc => acc.Role)
            .FirstOrDefaultAsync(acc => acc.Login == request.Username, cancellationToken);
        if (account == null) return null;
        var verificationResult = _passwordHasher.VerifyHashedPassword(account, account.Password, request.Password);

        return verificationResult == PasswordVerificationResult.Success
            ? new AccountLoginResponse()
            {
                Username = account.Login,
                Role = account.Role.Name
            }
            : null;
    }
}