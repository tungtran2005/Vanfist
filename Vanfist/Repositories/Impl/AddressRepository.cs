using Microsoft.EntityFrameworkCore;
using Vanfist.Configuration.Database;
using Vanfist.Entities;

namespace Vanfist.Repositories.Impl;

public class AddressRepository : Repository<Address>, IAddressRepository
{
    public AddressRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Address?> FindByDefaultAndAccountId(int accountId)
    {
        return await _context.Addresses
            .FirstOrDefaultAsync(a => a.IsDefault && a.AccountId == accountId);
    }
}