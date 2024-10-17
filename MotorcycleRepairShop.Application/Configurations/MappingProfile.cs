﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MotorcycleRepairShop.Application.Model.Account;
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
        }
    }
}
