using BookService.Domain.Entities.Orders.Entity;
using BookService.Domain.Entities.Users.Entity;
using BookService.Infrastructure.Persistence.Repositories.Custom;
using BookService.Infrastructure.Persistence.Repositories.Default;
using BookService.Infrastructure.Persistence.UnitOfWorks.Default;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookService.Persistance.Repositories.Custom
{
    public interface IOrderRepository : IRepository<OrderEntity>
    {
        Task<OrderEntity?> FindAsync(int id);
        Task<List<OrderEntity>> FindByUserIdAsync(int UserId);
    }
    public sealed class OrderRepository: Repository<OrderEntity>, IOrderRepository
    {
        public OrderRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<OrderEntity?> FindAsync(int id)
        {
            return await FirstOrDefaultAsync(x => x.Id == id);
            
        }

        public async Task<List<OrderEntity>> FindByUserIdAsync(int UserId)
        {
            return await Where(x => x.UserId == UserId).ToListAsync();
        }
        public async Task<List<OrderEntity>> FindByDatePeriodAsync(DateTime startDate, DateTime endDate)
        {
            return await Where(x => x.CreatedAt.Date >= startDate.Date && x.CreatedAt.Date <= endDate.Date)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();
        }
    }
}
