using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Application.Abstracts.Services;
using TeamProject.Domain.Entities;
using TeamProject.Application.DTOs.AuthDTOs;
using Microsoft.Extensions.Options;
using TeamProject.Application.Options;

namespace TeamProject.Persistence.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtTokenGenerator _jwtGenerator;
    private readonly IRefreshTokenService _refreshTokenService; 
    private readonly JwtOptions _jwtOptions; 

    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IJwtTokenGenerator jwtGenerator,
        IRefreshTokenService refreshTokenService, 
        IOptions<JwtOptions> jwtOptions) 
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtGenerator = jwtGenerator;
        _refreshTokenService = refreshTokenService;
        _jwtOptions = jwtOptions.Value;
    }

    private async Task<TokenResponse> BuildTokenResponseAsync(User user)
    {
        var accessToken = _jwtGenerator.GenerateAccessToken(user);
        var refreshToken = await _refreshTokenService.CreateAsync(user);

        return new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAtUtc = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes)
        };
    }

    public async Task<(bool Success, string? Error)> RegisterAsync(TeamProject.Application.DTOs.AuthDTOs.RegisterRequest request, CancellationToken ct = default)
    {
        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email,
            FullName = request.FullName
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return (false, errors);
        }

        return (true, null);
    }

    public async Task<TokenResponse?> LoginAsync(TeamProject.Application.DTOs.AuthDTOs.LoginRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Login)
                    ?? await _userManager.FindByNameAsync(request.Login);

        if (user == null) return null;

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
        if (!result.Succeeded) return null;

        return await BuildTokenResponseAsync(user);
    }

    public async Task<TokenResponse?> RefreshTokenAsync(string refreshToken)
    {
        var user = await _refreshTokenService.ValidateAndConsumeAsync(refreshToken);

        if (user == null) return null;

        return await BuildTokenResponseAsync(user);
    }
}
