using Vanfist.Entities;

namespace Vanfist.Repositories;

public interface IAddressRepository : IRepository<Address>
{
    Task<Address?> FindByDefaultAndAccountId(int accountId);
}