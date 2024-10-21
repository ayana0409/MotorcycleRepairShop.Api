using AutoMapper;
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
                .ForMember(
                    dest => dest.UserRoles, 
                    opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.RoleId).ToList()));

            #region Service

            CreateMap<ServiceDto, Service>().ReverseMap();
            CreateMap<ServiceTableDto, Service>().ReverseMap();

            #endregion

            #region Brand

            CreateMap<BrandDto, Brand>().ReverseMap();
            CreateMap<BrandTableDto, Brand>().ReverseMap();

            #endregion

            #region Vehicle

            CreateMap<CreateVehicleDto, Vehicle>()
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<Vehicle, VehicleDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => i.Name).ToList()));
            CreateMap<VehicleDto, Vehicle>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());
            CreateMap<UpdateVehicleDto, Vehicle>()
                .ForMember(dest => dest.Images, opts => opts.Ignore());
            CreateMap<Vehicle, VehicleTableDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => i.Name).ToList()));

            #endregion

            #region Part

            CreateMap<PartDto, Part>().ReverseMap();
            CreateMap<PartTableDto, Part>().ReverseMap();

            #endregion
        }
    }
}
