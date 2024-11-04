using MotorcycleRepairShop.Application.Interfaces.Repositories;
using MotorcycleRepairShop.Domain.Entities;
using MotorcycleRepairShop.Infrastructure.Persistence;

namespace MotorcycleRepairShop.Infrastructure.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        
    }
}
