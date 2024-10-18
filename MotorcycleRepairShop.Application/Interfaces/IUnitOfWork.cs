using Microsoft.EntityFrameworkCore;
using MotorcycleRepairShop.Application.Interfaces.Repositories;

namespace MotorcycleRepairShop.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IServiceRepository ServiceRepository { get; }

        Task BeginTransaction();
        Task CommitTransaction();
        Task RollBackTransaction();
        Task SaveChangeAsync();
        DbSet<T> Table<T>() where T : class;
    }
}
