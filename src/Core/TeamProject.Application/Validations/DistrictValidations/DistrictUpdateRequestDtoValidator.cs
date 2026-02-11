using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Application.DTOs.DistrictDTOs;

namespace TeamProject.Application.Validations.DistrictValidations;

public class DistrictUpdateRequestDtoValidator:AbstractValidator<DistrictUpdateRequestDto>
{
    public DistrictUpdateRequestDtoValidator()
    {
        RuleFor(x => x.Name)
               .NotEmpty().WithMessage("District name is required.")
               .MinimumLength(2).WithMessage("District name must be at least 2 characters long.")
               .MaximumLength(100).WithMessage("District name cannot exceed 100 characters.");

        RuleFor(x => x.CityId)
            .NotEmpty().WithMessage("City reference is required.")
            .GreaterThan(0).WithMessage("Please provide a valid City ID.");
    }
}
