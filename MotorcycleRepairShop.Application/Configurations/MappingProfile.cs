using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Application.Model.Account;
using MotorcycleRepairShop.Application.Model.Service;
using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Application.Configurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateAccountDto, ApplicationUser>()
            .ForMember(dest => dest.UserRoles, opt => opt.Ignore());
            CreateMap<ApplicationUser, CreateAccountDto>()
            .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.RoleId).ToList()));

            CreateMap<ServiceDto, Service>().ReverseMap();
            CreateMap<ServiceTableDto, Service>().ReverseMap();

            CreateMap<BrandDto, Brand>().ReverseMap();
            CreateMap<BrandTableDto, Brand>().ReverseMap();
        }
    }
}
