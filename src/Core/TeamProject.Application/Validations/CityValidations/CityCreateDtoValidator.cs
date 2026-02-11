using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Application.DTOs.CityDTOs;

namespace TeamProject.Application.Validations.CityValidations;

public class CityCreateDtoValidator : AbstractValidator<CityCreateRequestDto>
{
    public CityCreateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("The name of the city cannot empty")
            .MaximumLength(100).WithMessage("The name of the city cannot contains of more than 100 symbols"); 

        RuleFor(x => x.Area)
            .GreaterThan(0).WithMessage("The area of the city must be more than 0"); 
    }
}
