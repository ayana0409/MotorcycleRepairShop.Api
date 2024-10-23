using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MotorcycleRepairShop.Application.Interfaces;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Share.Exceptions;
using MySqlX.XDevAPI.Common;
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
                .FirstOrDefaultAsync(r => r.Id.Equals(id))
                ?? throw new NotFoundException(nameof(ServiceRequest), id);

            var result = _mapper.Map<ServiceRequestDto>(service);
            LogEnd(id);
            return result;
        }

        public async Task<int> CreateDeirecServiceRequest(ServiceRequestDto serviceRequestDto)
            => await CreateServiceRequest(serviceRequestDto);

        private async Task<int> CreateServiceRequest(ServiceRequestDto serviceRequestDto)
        {
            LogStart();
            var service = _mapper.Map<ServiceRequest>(serviceRequestDto);


            await _unitOfWork.ServiceRequestRepository.CreateAsync(service);
            await _unitOfWork.SaveChangeAsync();
            var result = service.Id;

            List<ServiceRequestProblem> problems = [];
            foreach (var problemId in serviceRequestDto.Problems)
                problems.Add(new() { ServiceRequestId = result, ProblemId = problemId });
            await _unitOfWork.Table<ServiceRequestProblem>()
                .AddRangeAsync(problems);
            await _unitOfWork.SaveChangeAsync();

            if (serviceRequestDto.Images.Any())
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
            LogEnd();
            return result;
        }

    }
}
