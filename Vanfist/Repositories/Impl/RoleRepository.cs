using Vanfist.Configuration;
using Vanfist.Entities;

namespace Vanfist.Repositories.Impl;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }
}