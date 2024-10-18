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
        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public DbSet<T> Table<T>() where T : class => _applicationDbContext.Set<T>();

        public IServiceRepository ServiceRepository => _serviceRepository ??= new ServiceRepository(_applicationDbContext);
       
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
