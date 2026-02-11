using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TeamProject.Application.DTOs.DistrictDTOs;

namespace TeamProject.Application.Validations.DistrictValidations;
public class DistrictCreateRequestDtoValidator:AbstractValidator<DistrictCreateRequestDto>
{
    public DistrictCreateRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("District name is required.")
            .MinimumLength(2).WithMessage("District name must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("District name cannot exceed 100 characters.")
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("District name cannot consist of only spaces.");

        RuleFor(x => x.CityId)
            .NotEmpty().WithMessage("City selection is required.")
            .GreaterThan(0).WithMessage("Please select a valid city.");
    }
}
