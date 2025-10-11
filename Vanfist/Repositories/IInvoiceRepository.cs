using Vanfist.Entities;

namespace Vanfist.Repositories
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        Task<IEnumerable<Invoice>> FindByAccountId(int accountId);
        IQueryable<Invoice> Query();
    }
}
