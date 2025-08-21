using Vanfist.DTOs.Requests;
using Vanfist.DTOs.Responses;
using Vanfist.Services.Base;

namespace Vanfist.Services;

public interface IAuthService : IService
{
    Task<AccountResponse> Register(RegisterRequest request);
    Task<AccountResponse> Login(LoginRequest request);
}