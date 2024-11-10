using AutoMapper;
using MotorcycleRepairShop.Application.Interfaces;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Application.Model.Service;
using Serilog;

namespace MotorcycleRepairShop.Infrastructure.Services
{
    public class HomeService : BaseService, IHomeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public HomeService(ILogger logger, IUnitOfWork unitOfWork, IMapper mapper) : base(logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ServiceHomeDto>> GetServiceList()
        {
            var services = await _unitOfWork.ServiceRepository
                .GetAllAsync(s => s.IsActive == true);
            var result = _mapper.Map<IEnumerable<ServiceHomeDto>>(services);
            return result;
        }

        public async Task<IEnumerable<PartHomeDto>> GetPartList()
        {
            var parts = await _unitOfWork.PartRepository
                .GetAllIncludeBrand();
            var result = _mapper.Map<IEnumerable<PartHomeDto>>(parts);
            return result;
        }
    }
}
