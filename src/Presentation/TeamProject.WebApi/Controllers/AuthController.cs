using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamProject.Application.Abstracts.Services;
using TeamProject.Application.Common;
using TeamProject.Application.DTOs.AuthDTOs;

namespace TeamProject.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var (success, error) = await _authService.RegisterAsync(request, ct);

        if (!success)
        {
            return BadRequest(BaseResponse.Fail(error!));
        }

        return Ok(BaseResponse.Success("User registered successfully."));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var (response, error) = await _authService.LoginAsync(request, ct);

        if (error != null)
        {
            // Email təsdiqlənməyibsə və ya şifrə səhvdirsə, bura işləyəcək
            return Unauthorized(BaseResponse.Fail(error));
        }

        return Ok(BaseResponse<TokenResponse>.Success(response!));
    }

    [HttpPost("refresh")] 
    [AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return BadRequest(BaseResponse.Fail("Refresh token is required."));
        }

        var response = await _authService.RefreshTokenAsync(request.RefreshToken);

        if (response == null)
        {
            return Unauthorized(BaseResponse.Fail("Invalid or expired refresh token."));
        }

        return Ok(BaseResponse<TokenResponse>.Success(response));
    }
    [HttpGet("confirm-email")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
        {
            return BadRequest(BaseResponse.Fail("İstifadəçi ID-si və ya token təqdim edilməyib."));
        }

        var isConfirmed = await _authService.ConfirmEmailAsync(userId, token);

        if (!isConfirmed)
        {
            return BadRequest(BaseResponse.Fail("E-poçt təsdiqi uğursuz oldu. Token etibarsız ola bilər və ya vaxtı keçib."));
        }

        return Ok(BaseResponse.Success("E-poçt ünvanınız uğurla təsdiqləndi. İndi sistemə giriş edə bilərsiniz."));
    }
}

