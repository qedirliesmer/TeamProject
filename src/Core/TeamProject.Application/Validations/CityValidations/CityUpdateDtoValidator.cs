using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Application.Abstracts.Repositories;
using TeamProject.Application.DTOs.CityDTOs;

namespace TeamProject.Application.Validations.CityValidations;

public class CityUpdateDtoValidator:AbstractValidator<CityUpdateRequestDto>
{
    public CityUpdateDtoValidator(ICityRepository cityRepository)
    {
        RuleFor(x => x.Name)
             .NotEmpty().WithMessage("The name of the city cannot empty")
             .MaximumLength(100).WithMessage("The name of the city cannot contains of more than 100 symbols")
             .MustAsync(async(request, name, ct)=>
             !await cityRepository.ExistsByNameAsync(name, request.Id, ct))
             .WithMessage("The city name is already exists");    

    }
}
