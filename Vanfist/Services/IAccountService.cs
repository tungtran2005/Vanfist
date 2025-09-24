using Vanfist.DTOs.Requests;
using Vanfist.DTOs.Responses;
using Vanfist.Services.Base;

namespace Vanfist.Services;

public interface IAccountService : IService
{
    Task<AccountResponse> GetCurrentAccount();
    Task<AccountResponse> FindById(int id);
    Task<AccountResponse> UpdateInformation(UpdateAccountRequest request);
    Task ChangePassword(ChangePasswordRequest request);
}