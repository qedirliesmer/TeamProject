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
using TeamProject.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TeamProject.Persistence.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtTokenGenerator _jwtGenerator;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly JwtOptions _jwtOptions;
    private readonly IEmailService _emailService;
    private readonly EmailOptions _emailOptions;

    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IJwtTokenGenerator jwtGenerator,
        IRefreshTokenService refreshTokenService,
        IOptions<JwtOptions> jwtOptions,
        IEmailService emailService,
    IOptions<EmailOptions> emailOptions)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtGenerator = jwtGenerator;
        _refreshTokenService = refreshTokenService;
        _jwtOptions = jwtOptions.Value;
        _emailService = emailService;
        _emailOptions = emailOptions.Value;
    }

    private async Task<TokenResponse> BuildTokenResponseAsync(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var accessToken = _jwtGenerator.GenerateAccessToken(user, roles);

        var refreshToken = await _refreshTokenService.CreateAsync(user);

        return new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAtUtc = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes)
        };
    }

    public async Task<(bool Success, string? Error)> RegisterAsync(Application.DTOs.AuthDTOs.RegisterRequest request, CancellationToken ct = default)
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

        await _userManager.AddToRoleAsync(user, RoleNames.User);

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var encodedToken = Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlEncode(
            System.Text.Encoding.UTF8.GetBytes(token));

        var baseUrl = _emailOptions.ConfirmationBaseUrl.TrimEnd('/');
        var confirmationLink = $"{baseUrl}?userId={user.Id}&token={encodedToken}";

        var subject = "E-poçt ünvanınızı təsdiqləyin";
        var htmlBody = $@"<h2>Xoş gəldiniz, {user.FullName}!</h2>
                     <p>Qeydiyyatı tamamlamaq üçün aşağıdakı linkə keçid edin:</p>
                     <a href='{confirmationLink}' style='padding:10px; background:blue; color:white;'>E-poçtu Təsdiqlə</a>";

        var plainText = $"Zəhmət olmasa bu linkə keçid edərək qeydiyyatı tamamlayın: {confirmationLink}";

        await _emailService.SendEmailAsync(user.Email!, subject, htmlBody, plainText);

        return (true, null);
    }

    public async Task<(TokenResponse? Response, string? Error)> LoginAsync(Application.DTOs.AuthDTOs.LoginRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Login)
                    ?? await _userManager.FindByNameAsync(request.Login);

        if (user == null)
            return (null, "İstifadəçi adı və ya şifrə yanlışdır.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);

        if (!result.Succeeded)
            return (null, "İstifadəçi adı və ya şifrə yanlışdır.");

        if (!user.EmailConfirmed)
        {
            return (null, "E-poçt ünvanınız təsdiqlənməyib. Zəhmət olmasa emailinizi yoxlayın.");
        }

        var tokenResponse = await BuildTokenResponseAsync(user);
        return (tokenResponse, null);
    }
    public async Task<bool> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        try
        {
            var decodedTokenBytes = Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlDecode(token);
            var decodedToken = System.Text.Encoding.UTF8.GetString(decodedTokenBytes);

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            return result.Succeeded;
        }
        catch
        {
            return false;
        }
    }
    public async Task<TokenResponse?> RefreshTokenAsync(string refreshToken)
    {
        var user = await _refreshTokenService.ValidateAndConsumeAsync(refreshToken);

        if (user == null) return null;

        return await BuildTokenResponseAsync(user);
    }
}
