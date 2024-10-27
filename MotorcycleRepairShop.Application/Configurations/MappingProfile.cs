using AutoMapper;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Application.Model.Account;
using MotorcycleRepairShop.Application.Model.Service;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Domain.Enums;
using MotorcycleRepairShop.Share.Extensions;

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

            #region Problem

            CreateMap<ProblemDto, Problem>().ReverseMap();
            CreateMap<ProblemTableDto, Problem>().ReverseMap();

            #endregion

            #region Part Inventory

            CreateMap<PartInventory, PartInventoryDto>().ReverseMap();
            //CreateMap<List<PartInventory>, List<PartInventoryDto>>();

            #endregion

            #region Service Request

            CreateMap<ServiceRequestUserInfoDto, ServiceRequest>();

            CreateMap<ServiceRequest, ServiceRequestDto>()
                .ForMember(
                    dest => dest.ServiceType, 
                    opt => opt.MapFrom(src => src.ServiceType.GetDisplayName()))
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => Enum.GetName(typeof(StatusEnum), src.StatusId)))
                .ForMember(
                    dest => dest.Images,
                    opt => opt.MapFrom(src => src.Images.Select(i => i.Name).ToList()))
                .ForMember(
                    dest => dest.Videos,
                    opt => opt.MapFrom(src => src.Videos.Select(i => i.Name).ToList()))
                .ForMember(
                    dest => dest.Problems,
                    otp => otp.Ignore())
                .ForMember(
                    dest => dest.Services,
                    opt => opt.Ignore());

            CreateMap<CreateServiceRequestDto, ServiceRequest>()
                .ForMember(
                    dest => dest.Images,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.Videos,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.Problems,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.Services,
                    opt => opt.Ignore());

            #endregion

            #region Service Request Item

            CreateMap<UpsSertServiceRequestItemDto, ServiceRequestItem>();
            CreateMap<ServiceRequestItem, ServiceRequestItemDto>();

            #endregion
        }
    }
}
