using Microsoft.EntityFrameworkCore;
using Vanfist.Configuration.Database;
using Vanfist.Entities;

namespace Vanfist.Repositories.Impl;

public class AccountRepository : Repository<Account>, IAccountRepository
{
    public AccountRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Account?> FindByEmail(string email)
    {
        return await _context.Accounts
            .Include(a => a.Roles)
            .Include(a => a.Addresses)
            .Include(a => a.Invoices)
            .FirstOrDefaultAsync(a => a.Email == email);
    }
}