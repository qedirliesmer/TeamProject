using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Application.Abstracts.Repositories;
using TeamProject.Application.Abstracts.Services;
using TeamProject.Application.Options;
using TeamProject.Domain.Entities;

namespace TeamProject.Persistence.Services;

public class RefreshTokenService:IRefreshTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly JwtOptions _jwtOptions;

    public RefreshTokenService(
        IRefreshTokenRepository refreshTokenRepository,
        IOptions<JwtOptions> jwtOptions)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<string> CreateAsync(User user)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var token = Convert.ToHexString(randomNumber);

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = token,
            UserId = user.Id,
            CreatedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = DateTime.UtcNow.AddMinutes(_jwtOptions.RefreshExpirationMinutes)
        };
        await _refreshTokenRepository.AddAsync(refreshToken);

        return token;
    }

    public async Task<User?> ValidateAndConsumeAsync(string token)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenWithUserAsync(token);

        if (refreshToken == null || refreshToken.ExpiresAtUtc < DateTime.UtcNow)
        {
            return null;
        }

        await _refreshTokenRepository.DeleteByTokenAsync(token);

        return refreshToken.User;
    }
}
