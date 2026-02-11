using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Application.DTOs.PropertyAdDTOs;
using TeamProject.Application.DTOs.PropertyMediaDTOs;
using TeamProject.Domain.Entities;

namespace TeamProject.Application.Mappings;

public class PropertyMediaProfile: Profile
{
    public PropertyMediaProfile()
    {
        CreateMap<PropertyAdCreateDto, PropertyAd>();
        CreateMap<PropertyMedia, PropertyMediaItemDto>();

        CreateMap<PropertyAd, GetAllPropertyAdResponse>()
            .ForMember(dest => dest.FirstMediaKey, opt => opt.MapFrom(src =>
                src.MediaItems.OrderBy(m => m.Order)
                             .Select(m => m.ObjectKey)
                             .FirstOrDefault()));

        CreateMap<PropertyAd, GetByIdPropertyAdResponse>()
            .ForMember(dest => dest.Media, opt => opt.MapFrom(src =>
                src.MediaItems.OrderBy(m => m.Order).ToList()));
    }
}
