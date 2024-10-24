﻿using MotorcycleRepairShop.Domain.Entities;

namespace MotorcycleRepairShop.Application.Interfaces.Repositories
{
    public interface IImageRepository : IBaseRepository<Image>
    {
        Task<IEnumerable<Image>> GetByVehicleIdAsync(int id);
    }
}
