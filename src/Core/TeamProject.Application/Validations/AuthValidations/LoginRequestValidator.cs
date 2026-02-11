using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Application.DTOs.AuthDTOs;

namespace TeamProject.Application.Validations.AuthValidations;

public class LoginRequestValidator:AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("Username or Email is required.")
            .MaximumLength(256).WithMessage("Login cannot exceed 256 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MaximumLength(256).WithMessage("Password cannot exceed 256 characters.");
    }
}
