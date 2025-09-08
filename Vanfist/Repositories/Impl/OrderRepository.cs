using Microsoft.EntityFrameworkCore;
using Vanfist.Configuration.Database;
using Vanfist.Entities;

namespace Vanfist.Repositories.Impl
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Order?> GetOrderWithDetails(int id)
        {
            return await _dbSet
                .Include(o => o.Account)
                .Include(o => o.Item)
                    .ThenInclude(i => i.Model)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> GetOrdersByAccount(int accountId)
        {
            return await _dbSet
                .Include(o => o.Item)
                    .ThenInclude(i => i.Model)
                .Where(o => o.AccountId == accountId)
                .ToListAsync();
        }
    }
}
