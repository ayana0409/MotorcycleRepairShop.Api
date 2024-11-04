using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MotorcycleRepairShop.Application.Interfaces;
using MotorcycleRepairShop.Application.Interfaces.Repositories;
using MotorcycleRepairShop.Infrastructure.Persistence;

namespace MotorcycleRepairShop.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private IDbContextTransaction _dbContextTransaction;
        private IServiceRepository? _serviceRepository;
        private IBrandRepository? _brandRepository;
        private IImageRepository? _imageRepository;
        private IVideoRepository? _videoRepository;
        private IVehicleRepository? _vehicleRepository;
        private IPaymentRepository? _paymentRepository;
        private IPartRepository? _partRepository;
        private IProblemRepository? _problemRepository;
        private IPartInventoryRepository _partInventoryRepository;
        private IServiceRequestRepository _serviceRequestRepository;
        private IServiceRequestItemRepository _serviceRequestItemRepository;
        private IServiceRequestPartRepository _serviceRequestPartRepository;

        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public DbSet<T> Table<T>() where T : class => _applicationDbContext.Set<T>();

        public IServiceRepository ServiceRepository => _serviceRepository ??= new ServiceRepository(_applicationDbContext);
        public IBrandRepository BrandRepository => _brandRepository ??= new BrandRepository(_applicationDbContext);
        public IImageRepository ImageRepository => _imageRepository ??= new ImageRepository(_applicationDbContext);
        public IVideoRepository VideoRepository => _videoRepository ??= new VideoRepository(_applicationDbContext);
        public IVehicleRepository VehicleRepository => _vehicleRepository ??= new VehicleRepository(_applicationDbContext);
        public IPartRepository PartRepository => _partRepository ??= new PartRepository(_applicationDbContext);
        public IProblemRepository ProblemRepository => _problemRepository ??= new ProblemRepository(_applicationDbContext);
        public IPaymentRepository PaymentRepository => _paymentRepository ??= new PaymentRepository(_applicationDbContext);
        public IPartInventoryRepository PartInventoryRepository => _partInventoryRepository ??= new PartInventoryRepository(_applicationDbContext);
        public IServiceRequestRepository ServiceRequestRepository => _serviceRequestRepository ??= new ServiceRequestRepository(_applicationDbContext);
        public IServiceRequestItemRepository ServiceRequestItemRepository => _serviceRequestItemRepository ??= new ServiceRequestItemRepository(_applicationDbContext);
        public IServiceRequestPartRepository ServiceRequestPartRepository => _serviceRequestPartRepository ??= new ServiceRequestPartRepository(_applicationDbContext);

        #region Transaction
        public async Task BeginTransaction()
        {
            _dbContextTransaction = await _applicationDbContext.Database.BeginTransactionAsync();
        }
        
        public async Task CommitTransaction()
        {
            await _dbContextTransaction?.CommitAsync();
        }

        public async Task RollBackTransaction()
        {
            await _dbContextTransaction?.RollbackAsync();
        }

        public async Task SaveChangeAsync()
        {
            await _applicationDbContext?.SaveChangesAsync();
        }
        #endregion
        public void Dispose()
        {
            if (_applicationDbContext != null)
            {
                _applicationDbContext.Dispose();
            }
        }
    }
}
