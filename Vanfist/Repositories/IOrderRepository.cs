using Vanfist.Entities;

namespace Vanfist.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderWithDetails(int id);
        Task<List<Order>> GetOrdersByAccount(int accountId);
    }
}
