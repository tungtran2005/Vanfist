using Vanfist.Entities;
using Vanfist.Services.Base;

namespace Vanfist.Services;

public interface IAddressService : IService
{
    Task<Address> FindByDefault();
}