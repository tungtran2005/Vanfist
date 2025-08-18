using Vanfist.DTOs.Responses;
using Vanfist.Services.Base;
using LoginRequest = Vanfist.DTOs.Requests.LoginRequest;
using RegisterRequest = Vanfist.DTOs.Requests.RegisterRequest;

namespace Vanfist.Services;

public interface IAuthService : IService
{
    Task<AccountResponse> Register(RegisterRequest request);
    Task<AccountResponse> Login(LoginRequest request);
}