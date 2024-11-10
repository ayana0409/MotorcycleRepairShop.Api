using AutoMapper;
using MotorcycleRepairShop.Application.Interfaces;
using MotorcycleRepairShop.Application.Interfaces.Services;
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

        public async Task<IEnumerable<ServiceDto>> GetServiceList()
        {
            var services = await _unitOfWork.ServiceRepository
                .GetAllAsync(s => s.IsActive == true);
            var result = _mapper.Map<IEnumerable<ServiceDto>>(services);
            return result;
        }
    }
}
