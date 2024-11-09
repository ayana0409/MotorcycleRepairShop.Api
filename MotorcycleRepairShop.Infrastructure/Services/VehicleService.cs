using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MotorcycleRepairShop.Application.Interfaces;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Share.Exceptions;
using PayPalCheckoutSdk.Orders;
using Serilog;

namespace MotorcycleRepairShop.Infrastructure.Services
{
    public class VehicleService : BaseService, IVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService<Vehicle> _cloudinaryService;

        public VehicleService(IUnitOfWork unitOfWork, IMapper mapper, ILogger logger, ICloudinaryService<Vehicle> cloudinaryService)
            : base(logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }
        public async Task<TableResponse<VehicleTableDto>> GetVehiclePagination(TableRequest request)
        {
            LogStart();
            var (result, total) = await _unitOfWork.VehicleRepository
                .GetPanigationAsync(request.PageIndex, request.PageSize, request.Keyword ?? "");
            List<VehicleTableDto> datas = [];

            foreach (var item in result)
            {
                datas.Add(_mapper.Map<VehicleTableDto>(item));
            }

            LogEnd();
            return new TableResponse<VehicleTableDto>
            {
                PageSize = request.PageSize,
                Datas = datas,
                Total = total
            };
        }
        public async Task<VehicleDto> GetVehicleById(int id)
        {
            LogStart(id);
            var vehicle = await _unitOfWork.Table<Vehicle>()
                .Include(v => v.Images)
                .FirstOrDefaultAsync(x => x.Id.Equals(id))
                ?? throw new NotFoundException("Vehicle", id);
            LogEnd(id);

            var result = _mapper.Map<VehicleDto>(vehicle);
            return result;
        }

        public async Task<int> CreateVehicle(CreateVehicleDto vehicleDto)
        {
            LogStart();
            var vehicle = _mapper.Map<Vehicle>(vehicleDto);

            await _unitOfWork.Table<Vehicle>().AddAsync(vehicle);
            await _unitOfWork.SaveChangeAsync();

            var result = vehicle.Id;
            UploadVehicleImage(result, vehicleDto.Images);

            LogEnd(result);
            return result;
        }

        public async Task<VehicleDto> UpdateVehicle(int vehicleId, UpdateVehicleDto vehicleDto)
        {
            LogStart(vehicleId);
            var vehicle = await _unitOfWork.Table<Vehicle>()
                    .FirstOrDefaultAsync(v => v.Id.Equals(vehicleId))
                    ?? throw new NotFoundException(nameof(Vehicle), vehicleId);

            var updateVehicle = _mapper.Map(vehicleDto, vehicle);
            _unitOfWork.Table<Vehicle>().Update(updateVehicle);
            await _unitOfWork.SaveChangeAsync();

            var existImages = await _unitOfWork.ImageRepository
                .GetByVehicleIdAsync(vehicleId);

            var validImageUrls = vehicleDto.ImageUrls.Where(i => i.Contains("http://res.cloudinary.com/")).ToList();

            var deleteTasks = existImages
                                .Where(image => image.Name != null && !validImageUrls.Contains(image.Name))
                                .Select(image => {
                                    _unitOfWork.ImageRepository.Delete(image);
                                    return _cloudinaryService.DeletePhotoAsync(image.Name);
                                })
                                .ToArray();
            await Task.WhenAll(deleteTasks);
            await _unitOfWork.SaveChangeAsync();

            var insertImage = vehicleDto.Images.Where(i => i.FileName != "blob").ToList();
            UploadVehicleImage(vehicleId, insertImage);

            var result = _mapper.Map<VehicleDto>(updateVehicle);
            LogEnd(vehicleId);

            return result;
        }

        public async Task DeleteVehicle(int vehicleId)
        {
            LogStart(vehicleId);
            var vehicle = await _unitOfWork.Table<Vehicle>()
                .FirstOrDefaultAsync(v => v.Id.Equals(vehicleId))
                ?? throw new NotFoundException(nameof(Vehicle), vehicleId);

            await DeleteVehicleImage(vehicleId);
            vehicle.IsActive = false;
            _unitOfWork.Table<Vehicle>().Update(vehicle);
            _unitOfWork.SaveChangeAsync().GetAwaiter();
            LogEnd(vehicleId);
        }
        private void UploadVehicleImage(int vehicleId, List<IFormFile> images)
        {
            LogStart(vehicleId);
            List<Image> addImages = [];
            foreach (var item in images)
            {
                var imagePath = _cloudinaryService.UploadPhoto(item);
                addImages.Add(new() { Name = imagePath, VehicleId = vehicleId });
            }
            _unitOfWork.Table<Image>().AddRange(addImages);
            _unitOfWork.SaveChangeAsync().GetAwaiter();
            LogEnd(vehicleId);
        }

        private async Task DeleteVehicleImage(int vehicleId)
        {
            LogStart(vehicleId);
            var images = await _unitOfWork.ImageRepository
                .GetByVehicleIdAsync(vehicleId);

            foreach (var image in images)
                await _cloudinaryService.DeletePhotoAsync(image.Name);

            _unitOfWork.Table<Image>().RemoveRange(images);
            await _unitOfWork.SaveChangeAsync();
            LogEnd(vehicleId);
        }
    }
}
