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

namespace TeamProject.Persistence.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtTokenGenerator _jwtGenerator;

    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IJwtTokenGenerator jwtGenerator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtGenerator = jwtGenerator;
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

    public async Task<string?> LoginAsync(TeamProject.Application.DTOs.AuthDTOs.LoginRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Login)
                   ?? await _userManager.FindByNameAsync(request.Login);

        if (user == null) return null;

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);

        if (!result.Succeeded) return null;

        return _jwtGenerator.GenerateToken(user);
    }
}
