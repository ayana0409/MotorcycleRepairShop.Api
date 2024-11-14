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
    public class ServiceService : BaseService, IServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ServiceService(IUnitOfWork unitOfWork, IMapper mapper, ILogger logger) : base(logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TableResponse<ServiceTableDto>> GetPagination(TableRequest request)
        {
            var (result, total) = await _unitOfWork.ServiceRepository
                .GetPanigationAsync(request.PageIndex, request.PageSize, request.Keyword ?? "");
            
            var datas = _mapper.Map<IEnumerable<ServiceTableDto>>(result);

            return new TableResponse<ServiceTableDto>
            {
                PageSize = request.PageSize,
                Datas = datas,
                Total = total
            };
        }

        public async Task<ServiceDto> GetById(int id)
        {
            var service = await _unitOfWork.Table<Service>()
                .FirstOrDefaultAsync(s => s.Id.Equals(id));
            var result = _mapper.Map<ServiceDto>(service);
            return result;
        }

        public async Task<int> CreateService(ServiceDto serviceDto)
        {
            var service = _mapper.Map<Service>(serviceDto);

            await _unitOfWork.Table<Service>().AddAsync(service);
            await _unitOfWork.SaveChangeAsync();
            
            var result = service.Id;
            LogSuccess(result);
            return result;
        }

        public async Task<ServiceDto> UpdateService(int id, ServiceDto serviceDto)
        {
            var service = await _unitOfWork.Table<Service>()
                .FirstOrDefaultAsync(s => s.Id.Equals(id))
                ?? throw new NotFoundException(nameof(Service), id);

            var updateService = _mapper.Map(serviceDto, service);
            _unitOfWork.Table<Service>().Update(updateService);
            _unitOfWork.SaveChangeAsync().Wait();

            var result = _mapper.Map<ServiceDto>(updateService);
            LogSuccess(id);

            return result;
        }

        public async Task DeleteService(int id)
        {
            var service = await _unitOfWork.Table<Service>()
                .FirstOrDefaultAsync(s => s.Id.Equals(id))
                ?? throw new NotFoundException(nameof(Service), id);

            service.IsActive = false;
            _unitOfWork.Table<Service>().Update(service);
            _unitOfWork.SaveChangeAsync().Wait();
            LogSuccess(id);
        }
    }
}
