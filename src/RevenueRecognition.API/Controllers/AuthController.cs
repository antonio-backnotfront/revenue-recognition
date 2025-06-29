using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueRecognition.Application.DTOs.Auth;
using RevenueRecognition.Application.Services.Auth;
using RevenueRecognition.Application.Services.Token;

namespace RevenueRecognition.API.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _service;

    public AuthController(IAuthService service, ITokenService tokenService, ILogger<AuthController> logger)
    {
        _service = service;
        _logger = logger;
        _tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Auth(AccountLoginRequest request, CancellationToken ct)
    {
        try
        {
            var userData = await _service.ValidateLoginByUsernameAndPassword(request, ct); 
            if (userData == null)
            {
                _logger.LogInformation("Invalid username or password");
                return Unauthorized();
            }

            var tokens = new
            {
                AccessToken = _tokenService.GenerateToken(userData.Username, userData.Role)
            };
            return Ok(tokens);
        }
        catch (KeyNotFoundException)
        {
            _logger.LogError("Something went wrong during Authentication");
            return Unauthorized();
        }
    }
    
}