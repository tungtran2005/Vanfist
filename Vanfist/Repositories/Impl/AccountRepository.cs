using Vanfist.Configuration.Database;
using Vanfist.Entities;

namespace Vanfist.Repositories.Impl;

public class AccountRepository : Repository<Account>, IAccountRepository
{
    public AccountRepository(ApplicationDbContext context) : base(context)
    {
    }
}