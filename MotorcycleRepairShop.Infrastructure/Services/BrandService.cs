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

            List<BrandTableDto> result = [];

            foreach (var item in existList)
            {
                result.Add(_mapper.Map<BrandTableDto>(item));
            }

            return result;
        }
        public async Task<TableResponse<BrandTableDto>> GetBrandPagination(TableRequest request)
        {
            LogStart();
            var (result, total) = await _unitOfWork.BrandRepository
                .GetPanigationAsync(request.PageIndex, request.PageSize, request.Keyword ?? "");
            List<BrandTableDto> datas = [];

            foreach (var item in result)
            {
                datas.Add(_mapper.Map<BrandTableDto>(item));
            }

            LogEnd();
            return new TableResponse<BrandTableDto>
            {
                PageSize = request.PageSize,
                Datas = datas,
                Total = total
            };
        }

        public async Task<BrandDto> GetById(int id)
        {
            LogStart(id);
            var brand = await _unitOfWork.Table<Brand>()
                .FirstOrDefaultAsync(b => b.Id.Equals(id))
                ?? throw new NotFoundException("Brand", id);

            var result = _mapper.Map<BrandDto>(brand);
            LogEnd(id);
            return result;
        }

        public async Task<int> CreateBrand(BrandDto brandDto)
        {
            LogStart();
            var brand = _mapper.Map<Brand>(brandDto);
            await _unitOfWork.Table<Brand>().AddAsync(brand);
            await _unitOfWork.SaveChangeAsync();
            var result = brand.Id;
            LogEnd(result);

            return result;
        }

        public async Task<BrandDto> UpdateBrand(int id, BrandDto brandDto)
        {
            LogStart(id);
            var brand = await _unitOfWork.Table<Brand>()
                .FirstOrDefaultAsync(b => b.Id.Equals(id))
                ?? throw new NotFoundException("Brand", id);

            var updateBrand = _mapper.Map(brandDto, brand);
            _unitOfWork.Table<Brand>().Update(updateBrand);
            _unitOfWork.SaveChangeAsync().Wait();
            var result = _mapper.Map<BrandDto>(updateBrand);
            LogEnd(id);

            return result;
        }
        
        public async Task DeleteBrand(int id)
        {
            LogStart(id);
            var brand = await _unitOfWork.Table<Brand>()
                .FirstOrDefaultAsync(b => b.Id.Equals(id))
                ?? throw new NotFoundException("Brand", id);

            brand.IsActive = false;
            _unitOfWork.Table<Brand>().Update(brand);
            _unitOfWork.SaveChangeAsync().Wait();
            LogEnd(id);
        }
    }
}
