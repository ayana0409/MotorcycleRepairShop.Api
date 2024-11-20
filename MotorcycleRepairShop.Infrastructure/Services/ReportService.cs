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
    public class ReportService : BaseService, IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ReportService(ILogger logger, IUnitOfWork unitOfWork, IMapper mapper) : base(logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceRequestInvoiceDto> GetServiceRequestInvoiceById(int serviceRequestId)
        {
            var serviceRequest = await _unitOfWork.ServiceRequestRepository
                .GetQueryable()
                .Include(sr => sr.Services)
                .ThenInclude(s => s.Service)
                .Include(sr => sr.Parts)
                .ThenInclude(p => p.Part)
                .Include(sr => sr.Problems)
                .ThenInclude(p => p.Problem)
                .AsSplitQuery()
                .FirstOrDefaultAsync(sr => sr.Id == serviceRequestId 
                && sr.StatusId == (int)StatusEnum.Completed)
                ?? throw new NotFoundException(nameof(ServiceRequest), serviceRequestId);

            var result = _mapper.Map<ServiceRequestInvoiceDto>(serviceRequest);

            // Get part info
            result.Parts = _mapper.Map<List<ServiceRequestPartInfoDto>>(serviceRequest.Parts);
            // Get problem info
            result.Problems = _mapper.Map<List<string>>(serviceRequest.Problems);
            // Get service info
            result.Services = _mapper.Map<List<ServiceRequestItemInfo>>(serviceRequest.Services);
            return result;
        }

    }
}
