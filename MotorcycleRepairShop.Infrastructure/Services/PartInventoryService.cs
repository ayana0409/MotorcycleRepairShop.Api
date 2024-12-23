﻿using AutoMapper;
using MotorcycleRepairShop.Application.Interfaces;
using MotorcycleRepairShop.Application.Interfaces.Services;
using MotorcycleRepairShop.Application.Model;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Share.Exceptions;
using Serilog;

namespace MotorcycleRepairShop.Infrastructure.Services
{
    public class PartInventoryService : BaseService, IPartInventoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PartInventoryService(ILogger logger, IUnitOfWork unitOfWork, IMapper mapper) : base(logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PartInventoryDto>> GetAvailableInventoriesByPartId(int partId)
        {
            var part = await _unitOfWork.PartInventoryRepository
                .GetSigleAsync(p => p.PartId.Equals(partId))
                ?? throw new NotFoundException(nameof(Part), partId);

            var inventories = await _unitOfWork.PartInventoryRepository
                .GetAllAsync(i => i.PartId.Equals(partId) && i.QuantityInStock > 0);

            var sortedInventories = inventories
                .OrderBy(i => i.EntryDate)
                .ToList();

            var result = _mapper.Map<List<PartInventoryDto>>(inventories);
            return result;
        }

        public async Task<int> CreatePartInventory(CreatePartInventoryDto partInventoryDto)
        {
            var partId = partInventoryDto.PartId;
            var part = await _unitOfWork.PartRepository
                .GetSigleAsync(p => p.Id.Equals(partId) && p.IsActive == true)
                ?? throw new NotFoundException(nameof(Part), partId);

            var inventory = _mapper.Map<PartInventory>(partInventoryDto);
            inventory.QuantityInStock = inventory.QuantityReceived;
            await _unitOfWork.PartInventoryRepository
                .CreateAsync(inventory);
            await _unitOfWork.SaveChangeAsync();

            var result = inventory.Id;
            LogSuccess(partId);
            return result;
        }

        public async Task<List<int>> CreatePartInventories(List<CreatePartInventoryDto> partInventoryDtos)
        {
            if (partInventoryDtos == null || partInventoryDtos.Count == 0)
                throw new ArgumentException("No inventory data provided.");

            var addedInventories = new List<PartInventory>();
            await _unitOfWork.BeginTransaction();
            LogStart();
            try
            {
                foreach (var partInventoryDto in partInventoryDtos)
                {
                    var partId = partInventoryDto.PartId;

                    var part = await _unitOfWork.PartRepository
                        .GetSigleAsync(p => p.Id.Equals(partId) && p.IsActive == true)
                        ?? throw new NotFoundException(nameof(Part), partId);

                    var inventory = _mapper.Map<PartInventory>(partInventoryDto);
                    inventory.QuantityInStock = inventory.QuantityReceived;
                    await _unitOfWork.PartInventoryRepository.CreateAsync(inventory);

                    addedInventories.Add(inventory);
                    LogSuccess(partId);
                }

                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransaction();
            }
            catch (NotFoundException)
            {
                await _unitOfWork.RollBackTransaction();
                throw;
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackTransaction();
                throw;
            }

            var result = addedInventories.Select(i => i.Id).ToList();
            LogEnd();
            return result;
        }
    }
}
