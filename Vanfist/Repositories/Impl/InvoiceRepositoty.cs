using Microsoft.EntityFrameworkCore;
using Vanfist.Configuration.Database;
using Vanfist.Entities;

namespace Vanfist.Repositories.Impl
{
    public class InvoiceRepositoty : Repository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepositoty(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Invoice>> FindByAccountId(int accountId)
        {
            return await _dbSet.Where(i => i.AccountId == accountId).ToListAsync();
        }
    }
}
