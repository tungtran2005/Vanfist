using Microsoft.EntityFrameworkCore;
using Vanfist.Configuration.Database;
using Vanfist.Entities;

namespace Vanfist.Repositories.Impl;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<Role?> FindByName(string name)
    {
        return _dbSet.FirstOrDefaultAsync(r => r.Name == name);
    }

    public Task<bool> ExistsByName(string name)
    {
        return _dbSet.AnyAsync(r => r.Name == name);
    }
}