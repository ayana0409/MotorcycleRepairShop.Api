using Microsoft.EntityFrameworkCore;

namespace MotorcycleRepairShop.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task BeginTransaction();
        Task CommitTransaction();
        Task RollBackTransaction();
        Task SaveChangeAsync();
        DbSet<T> Table<T>() where T : class;
    }
}
