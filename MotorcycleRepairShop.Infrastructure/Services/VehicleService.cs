using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MotorcycleRepairShop.Application.Interfaces;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Share.Exceptions;
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

            await _unitOfWork.BeginTransaction();

            await _unitOfWork.Table<Vehicle>().AddAsync(vehicle);
            await _unitOfWork.SaveChangeAsync();

            var result = vehicle.Id;
            UploadVehicleImage(result, vehicleDto.Images);

            await _unitOfWork.CommitTransaction();
            LogEnd(result);

            return result;
        }

        private void UploadVehicleImage(int vehicleId, List<IFormFile> images)
        {
            List<Image> addImages = [];
            foreach (var item in images)
            {
                var imagePath = _cloudinaryService.UploadPhoto(item);
                addImages.Add(new() { Name = imagePath, VehicleId = vehicleId });
            }
            _unitOfWork.Table<Image>().AddRange(addImages);
            _unitOfWork.SaveChangeAsync();
        }
    }
}
