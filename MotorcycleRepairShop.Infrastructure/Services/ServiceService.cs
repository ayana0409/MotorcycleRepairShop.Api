using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorcycleRepairShop.Application.Interfaces;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Application.Model.Service;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Share.Exceptions;
using Serilog;

namespace MotorcycleRepairShop.Infrastructure.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public ServiceService(IUnitOfWork unitOfWork, IMapper mapper, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TableResponse<ServiceTableDto>> GetPagination(TableRequest request)
        {
            _logger.Information($"START - Get service pagination");

            var (result, total) = await _unitOfWork.ServiceRepository
                .GetPanigationAsync(request.PageIndex, request.PageSize, request.Keyword ?? "");
            List<ServiceTableDto> datas = [];

            foreach (var item in result)
            {
                datas.Add(_mapper.Map<ServiceTableDto>(item));
            }

            _logger.Information($"END - Get service pagination");
            return new TableResponse<ServiceTableDto>
            {
                PageSize = request.PageSize,
                Datas = datas,
                Total = total
            };
        }

        public async Task<ServiceDto> GetById(int id)
        {
            _logger.Information($"START - Get service by id: {id}");
            var service = await _unitOfWork.Table<Service>()
                .FirstOrDefaultAsync(s => s.Id.Equals(id));

            var result = _mapper.Map<ServiceDto>(service);
            _logger.Information($"END - Get service by id: {id}");
            
            return result;
        }

        public async Task<int> CreateService(ServiceDto serviceDto)
        {
            _logger.Information($"START - CreateService: {serviceDto.Name}");
            var service = _mapper.Map<Service>(serviceDto);

            await _unitOfWork.Table<Service>().AddAsync(service);
            await _unitOfWork.SaveChangeAsync();
            _logger.Information($"END - CreateService: {serviceDto.Name}");

            return service.Id;
        }

        public async Task<ServiceDto> UpdateService(int id, ServiceDto serviceDto)
        {
            _logger.Information($"START - Update service: {id}");
            var service = await _unitOfWork.Table<Service>()
                .FirstOrDefaultAsync(s => s.Id.Equals(id))
                ?? throw new NotFoundException("Service", id);

            var updateService = _mapper.Map(serviceDto, service);
            _unitOfWork.Table<Service>().Update(updateService);
            _unitOfWork.SaveChangeAsync().Wait();

            var result = _mapper.Map<ServiceDto>(updateService);
            _logger.Information($"END - Update service: {id}");

            return result;
        }

        public async Task DeleteService(int id)
        {
            _logger.Information($"START - delete service: {id}");

            var service = await _unitOfWork.Table<Service>()
                .FirstOrDefaultAsync(s => s.Id.Equals(id))
                ?? throw new NotFoundException("Service", id);

            _unitOfWork.Table<Service>().Remove(service);
            _unitOfWork.SaveChangeAsync().Wait();

            _logger.Information($"END - delete service: {id}");
        }
    }
}
