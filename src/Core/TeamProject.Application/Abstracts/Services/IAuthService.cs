using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Application.DTOs.AuthDTOs;

namespace TeamProject.Application.Abstracts.Services;

public interface IAuthService
{
    Task<(bool Success, string? Error)> RegisterAsync(DTOs.AuthDTOs.RegisterRequest request, CancellationToken ct = default);
    Task<(TokenResponse? Response, string? Error)> LoginAsync(Application.DTOs.AuthDTOs.LoginRequest request, CancellationToken ct = default);

    Task<TokenResponse?> RefreshTokenAsync(string refreshToken);
    Task<bool> ConfirmEmailAsync(string userId, string token);

}
