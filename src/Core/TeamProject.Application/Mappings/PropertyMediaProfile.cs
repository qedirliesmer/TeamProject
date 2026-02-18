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
        CreateMap<PropertyAdUpdateDto, PropertyAd>();

        CreateMap<PropertyAd, PropertyAdGetAllDto>();

        CreateMap<PropertyAd, GetAllPropertyAdResponse>()
            .ForMember(dest => dest.FirstMediaKey, opt => opt.MapFrom(src =>
                src.MediaItems != null
                    ? src.MediaItems.OrderBy(m => m.Order).Select(m => m.ObjectKey).FirstOrDefault()
                    : null));

        CreateMap<PropertyAd, PropertyAdGetByIdDto>()
            .ForMember(dest => dest.OfferTypeName, opt => opt.MapFrom(src => src.OfferType.ToString()))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.PropertyCategory.ToString()))
            .ForMember(dest => dest.OwnerFullName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : "Naməlum"));

        CreateMap<PropertyAd, GetByIdPropertyAdResponse>()
            .ForMember(dest => dest.Media, opt => opt.MapFrom(src =>
                src.MediaItems != null
                    ? src.MediaItems.OrderBy(m => m.Order).ToList()
                    : new List<PropertyMedia>()));
    }
}
