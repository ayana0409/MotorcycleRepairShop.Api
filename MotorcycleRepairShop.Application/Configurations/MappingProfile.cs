using AutoMapper;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Application.Model.Account;
using MotorcycleRepairShop.Application.Model.Problem;
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

            CreateMap<AccountInfoDto, ApplicationUser>()
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<ApplicationUser, AccountInfoDto>();
            CreateMap<ApplicationUser, AccountTableDto>();
            #region Service

            CreateMap<ServiceDto, Service>().ReverseMap();
            CreateMap<ServiceTableDto, Service>().ReverseMap();
            CreateMap<ServiceHomeDto, Service>().ReverseMap();
            CreateMap<Service, ServiceForDropdownDto>().ReverseMap();

            #endregion

            #region Brand

            CreateMap<BrandDto, Brand>().ReverseMap();
            CreateMap<BrandTableDto, Brand>().ReverseMap();
            CreateMap<Brand, BrandHomeDto>().ReverseMap();

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
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => i.Name).ToList()))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name));
            CreateMap<Vehicle, VehicleHomeDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => i.Name).ToList()))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name));



            #endregion

            #region Part

            CreateMap<PartDto, Part>().ReverseMap();
            CreateMap<PartTableDto, Part>();
            CreateMap<Part, PartForDropdownDto>();
            CreateMap<Part, PartTableDto>()
                .ForMember(dest => dest.BrandName,
                opt => opt.MapFrom(src => src.Brand == null ? string.Empty : src.Brand.Name));
            CreateMap<Part, PartHomeDto>()
                .ForMember(dest => dest.BrandName,
                opt => opt.MapFrom(src => src.Brand == null ? string.Empty : src.Brand.Name));

            #endregion

            #region Problem

            CreateMap<ProblemDto, Problem>().ReverseMap();
            CreateMap<ProblemTableDto, Problem>().ReverseMap();
            CreateMap<Problem, ProblemForDropdownDto>();

            #endregion

            #region Part Inventory

            CreateMap<PartInventory, CreatePartInventoryDto>().ReverseMap();
            CreateMap<PartInventory, PartInventoryDto>();

            #endregion

            #region Service Request

            CreateMap<ServiceRequestUserInfoDto, ServiceRequest>();

            CreateMap<ServiceRequest, ServiceRequestDto>()
                .ForMember(
                    dest => dest.ServiceType,
                    opt => opt.MapFrom(src => src.ServiceType.GetDisplayName()))
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => ((StatusEnum)src.StatusId).GetDisplayName()))
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
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.Parts,
                    opt => opt.Ignore());

            CreateMap<ServiceRequest, ServiceRequestInfoDto>()
                .ForMember(
                    dest => dest.ServiceType,
                    opt => opt.MapFrom(src => src.ServiceType.GetDisplayName()))
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => ((StatusEnum)src.StatusId).GetDisplayName()))
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
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.Parts,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.TotalPrice,
                    opt => opt.MapFrom(src => src.GetTotalPrice()))
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => ((StatusEnum)src.StatusId).GetDisplayName()));

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

            CreateMap<ServiceRequest, ServiceRequestTable>()
                .ForMember(
                    dest => dest.TotalPrice,
                    opt => opt.MapFrom(src => src.GetTotalPrice()))
                .ForMember(
                    dest => dest.ServiceType,
                    opt => opt.MapFrom(src => src.ServiceType.GetDisplayName()))
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => ((StatusEnum)src.StatusId).GetDisplayName()));


            CreateMap<ServiceRequest, ServiceRequestHomeDto>()
                .ForMember(
                    dest => dest.TotalPrice,
                    opt => opt.MapFrom(src => src.GetTotalPrice()))
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => ((StatusEnum)src.StatusId).GetDisplayName()))
                .ForMember(
                    dest => dest.Type,
                    opt => opt.MapFrom(src => src.ServiceType.GetDisplayName()));

            CreateMap<ServiceRequestProblem, string>()
                .ConstructUsing(p => p.Problem.Name ?? "");


            #endregion

            #region Service Request Item

            CreateMap<UpSertServiceRequestItemDto, ServiceRequestItem>();
            CreateMap<ServiceRequestItem, ServiceRequestItemDto>();
            CreateMap<ServiceRequestItem, ServiceRequestItemInfo>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ServiceId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Service.Name));
            #endregion

            #region Service Request Part

            CreateMap<UpSertServiceRequestPartDto, ServiceRequestPart>();
            CreateMap<ServiceRequestPart, ServiceRequestPartDto>();
            CreateMap<ServiceRequestPart, ServiceRequestPartInfoDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.PartId))
                .ForMember(dest => dest.Name, opt =>
                    opt.MapFrom(src => src.Part != null ? src.Part.Name : null))
                .ForMember(dest => dest.WarrantyTo, opt =>
                    opt.MapFrom(src => src.WarrantyTo.HasValue
                        ? src.WarrantyTo.Value.ToString("dd/MM/yyyy")
                        : null));

            #endregion

            #region Payment

            CreateMap<CreatePaymentDto, Payment>();
            CreateMap<Payment, PaymentDto>();

            #endregion

            #region Report

            CreateMap<ServiceRequest, ServiceRequestInvoiceDto>()
                .ForMember(dest => dest.DateSubmitted, 
                opt => opt.MapFrom(src => src.DateSubmitted.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.CompletionDate, 
                opt => opt.MapFrom(src => src.CompletionDate.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.Problems, opt => opt.Ignore())
                .ForMember(dest => dest.Parts, opt => opt.Ignore())
                .ForMember(dest => dest.Services, opt => opt.Ignore());
            

            #endregion
        }
    }
}
