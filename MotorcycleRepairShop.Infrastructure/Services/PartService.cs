using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MotorcycleRepairShop.Application.Interfaces;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Share.Exceptions;
using Serilog;

namespace MotorcycleRepairShop.Infrastructure.Services
{
    public class PartService : BaseService, IPartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PartService(ILogger logger, IUnitOfWork unitOfWork, IMapper mapper) : base(logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PartForDropdownDto>> GetPartsForDropDownList()
        {
            var existList = await _unitOfWork.PartRepository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<PartForDropdownDto>>(existList);
            return result;
        }
        public async Task<TableResponse<PartTableDto>> GetPartPagination(TableRequest request)
        {
            var (result, total) = await _unitOfWork.PartRepository
                .GetPanigationAsync(request.PageIndex, request.PageSize, request.Keyword ?? "");
            List<PartTableDto> datas = [];

            foreach (var item in result)
            {
                datas.Add(_mapper.Map<PartTableDto>(item));
            }

            return new TableResponse<PartTableDto>
            {
                PageSize = request.PageSize,
                Datas = datas,
                Total = total
            };
        }

        public async Task<PartDto> GetPartById(int partId)
        {
            var part = await _unitOfWork.Table<Part>()
                .FirstOrDefaultAsync(p => p.Id == partId)
                ?? throw new NotFoundException(nameof(Part), partId);
            var result = _mapper.Map<PartDto>(part);
            return result;
        }

        public async Task<int> CreatePart(PartDto partDto)
        {
            if (!await _unitOfWork.BrandRepository.AnyAsync(partDto.BrandId))
                throw new NotFoundException(nameof(Brand), partDto.BrandId);

            var createPart = _mapper.Map<Part>(partDto);
            await _unitOfWork.Table<Part>().AddAsync(createPart);
            await _unitOfWork.SaveChangeAsync();

            var result = createPart.Id;
            LogSuccess(result);
            return result;
        }

        public async Task<PartDto> UpdatePart(int id, PartDto partDto)
        {
            if (!await _unitOfWork.BrandRepository.AnyAsync(partDto.BrandId))
                throw new NotFoundException(nameof(Brand), partDto.BrandId);
            
            var part = await _unitOfWork.PartRepository
                .GetSigleAsync(p => p.Id.Equals(id))
                ?? throw new NotFoundException(nameof(Part), id);

            var updatePart = _mapper.Map(partDto, part);
            updatePart.UpdateDate = DateTime.UtcNow;
            _unitOfWork.PartRepository.Update(updatePart);
            await _unitOfWork.SaveChangeAsync();

            var result = _mapper.Map<PartDto>(updatePart);
            LogSuccess(id);
            return result;
        }

        public async Task DeletePart(int id)
        {
            var deletePart = await _unitOfWork.Table<Part>()
                .FirstOrDefaultAsync(p => p.Id.Equals(id))
                ?? throw new NotFoundException(nameof(Part), id);
            
            deletePart.UpdateDate = DateTime.UtcNow;
            deletePart.IsActive = false;
            _unitOfWork.Table<Part>().Update(deletePart);
            await _unitOfWork.SaveChangeAsync();
            LogSuccess(id);
        }
    }
}
