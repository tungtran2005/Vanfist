using Vanfist.Entities;

namespace Vanfist.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> FindByName(string name);
    Task<bool> ExistsByName(string name);
}