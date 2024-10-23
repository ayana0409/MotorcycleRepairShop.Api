using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MotorcycleRepairShop.Application.Interfaces;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Domain.Enums;
using MotorcycleRepairShop.Share.Exceptions;
using Serilog;

namespace MotorcycleRepairShop.Infrastructure.Services
{
    public class ServiceRequestService : BaseService, IServiceRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService<ServiceRequest> _cloudinaryService;
        public ServiceRequestService(ILogger logger, IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryService<ServiceRequest> cloudinaryService) : base(logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<ServiceRequestDto> GetServiceRequestById(int id)
        {
            LogStart(id);
            var service = await _unitOfWork.Table<ServiceRequest>()
                .Include(r => r.Images)
                .Include(r => r.Videos)
                .Include(r => r.Problems)
                .Include(r => r.Status)
                .AsSplitQuery()
                .FirstOrDefaultAsync(r => r.Id.Equals(id))
                ?? throw new NotFoundException(nameof(ServiceRequest), id);

            var result = _mapper.Map<ServiceRequestDto>(service);
            var problems = await _unitOfWork.ProblemRepository
                .GetByServideRequestId(id);
            result.Problems = problems.Select(p => p.Name).ToList();

            LogEnd(id);
            return result;
        }

        public async Task<int> CreateDeirecServiceRequest(CreateServiceRequestDto serviceRequestDto)
            => await CreateServiceRequest(serviceRequestDto, ServiceType.Direct);

        public async Task<int> CreateRemoteServiceRequest(CreateServiceRequestDto serviceRequestDto)
            => await CreateServiceRequest(serviceRequestDto, ServiceType.Remote);

        public async Task<int> CreateRescueRescueRequest(CreateServiceRequestDto serviceRequestDto)
            => await CreateServiceRequest(serviceRequestDto, ServiceType.Rescue);

        private async Task<int> CreateServiceRequest(CreateServiceRequestDto serviceRequestDto, ServiceType type)
        {
            LogStart($"CreateServiceRequest {Enum.GetName(type)}");
            var service = _mapper.Map<ServiceRequest>(serviceRequestDto);
            service.ServiceType = type;

            await _unitOfWork.ServiceRequestRepository.CreateAsync(service);
            await _unitOfWork.SaveChangeAsync();
            var result = service.Id;

            List<ServiceRequestProblem> problems = [];
            foreach (var problemId in serviceRequestDto.Problems)
                problems.Add(new() { ServiceRequestId = result, ProblemId = problemId });
            await _unitOfWork.Table<ServiceRequestProblem>()
                .AddRangeAsync(problems);
            await _unitOfWork.SaveChangeAsync();

            if (serviceRequestDto.Problems != null && serviceRequestDto.Images.Any())
            {
                foreach (var image in serviceRequestDto.Images)
                {
                    await _unitOfWork.ImageRepository.CreateAsync(new()
                    {
                        ServiceRequestId = result,
                        Name = _cloudinaryService.UploadPhoto(image)
                    });
                }
                await _unitOfWork.SaveChangeAsync();
            }
            LogEnd(result, $"CreateServiceRequest {Enum.GetName(type)}");
            return result;
        }

    }
}
