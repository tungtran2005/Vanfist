using Vanfist.Entities;

namespace Vanfist.Repositories;

public interface IAccountRepository : IRepository<Account>
{
    Task<Account?> FindByEmail(string email);
}