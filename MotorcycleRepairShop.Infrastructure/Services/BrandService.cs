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
    public class BrandService : BaseService, IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BrandService(ILogger logger, IUnitOfWork unitOfWork, IMapper mapper) : base(logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BrandTableDto>> GetBrandForDropDownList()
        {
            var existList = await _unitOfWork.BrandRepository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<BrandTableDto>>(existList);
            return result;
        }
        public async Task<TableResponse<BrandTableDto>> GetBrandPagination(TableRequest request)
        {
            var (result, total) = await _unitOfWork.BrandRepository
                .GetPanigationAsync(request.PageIndex, request.PageSize, request.Keyword ?? "");

            var datas = _mapper.Map<IEnumerable<BrandTableDto>>(result);
            return new TableResponse<BrandTableDto>
            {
                PageSize = request.PageSize,
                Datas = datas,
                Total = total
            };
        }

        public async Task<BrandDto> GetById(int id)
        {
            var brand = await _unitOfWork.Table<Brand>()
                .FirstOrDefaultAsync(b => b.Id.Equals(id))
                ?? throw new NotFoundException(nameof(Brand), id);
            var result = _mapper.Map<BrandDto>(brand);
            return result;
        }

        public async Task<int> CreateBrand(BrandDto brandDto)
        {
            var brand = _mapper.Map<Brand>(brandDto);
            await _unitOfWork.Table<Brand>().AddAsync(brand);
            await _unitOfWork.SaveChangeAsync();
            var result = brand.Id;
            LogSuccess(result);

            return result;
        }

        public async Task<BrandDto> UpdateBrand(int id, BrandDto brandDto)
        {
            var brand = await _unitOfWork.Table<Brand>()
                .FirstOrDefaultAsync(b => b.Id.Equals(id))
                ?? throw new NotFoundException(nameof(Brand), id);

            var updateBrand = _mapper.Map(brandDto, brand);
            _unitOfWork.Table<Brand>().Update(updateBrand);
            _unitOfWork.SaveChangeAsync().Wait();
            var result = _mapper.Map<BrandDto>(updateBrand);
            LogSuccess(id);

            return result;
        }
        
        public async Task DeleteBrand(int id)
        {
            var brand = await _unitOfWork.Table<Brand>()
                .FirstOrDefaultAsync(b => b.Id.Equals(id))
                ?? throw new NotFoundException(nameof(Brand), id);

            brand.IsActive = false;
            _unitOfWork.Table<Brand>().Update(brand);
            _unitOfWork.SaveChangeAsync().Wait();
            LogSuccess(id);
        }
    }
}
