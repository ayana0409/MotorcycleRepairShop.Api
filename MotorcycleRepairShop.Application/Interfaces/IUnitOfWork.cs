using Microsoft.EntityFrameworkCore;
using MotorcycleRepairShop.Application.Interfaces.Repositories;

namespace MotorcycleRepairShop.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IServiceRepository ServiceRepository { get; }
        IBrandRepository BrandRepository { get; }
        IImageRepository ImageRepository { get; }
        IVehicleRepository VehicleRepository { get; }
        IPartRepository PartRepository { get; }
        IProblemRepository ProblemRepository { get; }
        IPartInventoryRepository PartInventoryRepository { get; }
        IServiceRequestRepository ServiceRequestRepository { get; }
        IServiceRequestItemRepository ServiceRequestItemRepository { get; }
        IVideoRepository VideoRepository { get; }
        IServiceRequestPartRepository ServiceRequestPartRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        IStatisticRepository StatisticRepository { get; }

        Task BeginTransaction();
        Task CommitTransaction();
        Task RollBackTransaction();
        Task SaveChangeAsync();
        DbSet<T> Table<T>() where T : class;
    }
}
