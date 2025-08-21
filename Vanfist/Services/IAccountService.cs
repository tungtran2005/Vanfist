using Vanfist.DTOs.Requests;
using Vanfist.DTOs.Responses;
using Vanfist.Services.Base;

namespace Vanfist.Services;

public interface IAccountService : IService
{
    Task<AccountResponse> FindById(int id);
}