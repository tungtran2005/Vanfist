using Vanfist.Entities;
using Vanfist.Repositories;
using Vanfist.DTOs.Responses;
using LoginRequest = Vanfist.DTOs.Requests.LoginRequest;
using RegisterRequest = Vanfist.DTOs.Requests.RegisterRequest;
using Vanfist.Configuration.Database;
using Vanfist.Services.Base;
using Vanfist.Utils;

namespace Vanfist.Services.Impl;

public class AuthService : Service, IAuthService
{
    private readonly ICookieService _cookieService;
    private readonly IAccountRepository _accountRepository;
    private readonly IRoleRepository _roleRepository;

    public AuthService(
        ICookieService cookieService,
        IAccountRepository accountRepository,
        IRoleRepository roleRepository)
    {
        _cookieService = cookieService;
        _accountRepository = accountRepository;
        _roleRepository = roleRepository;
    }
    
    public async Task<AccountResponse> Register(RegisterRequest request)
    {
        var existingAccount = await _accountRepository.FindByEmail(request.Email);
        if (existingAccount != null)
        {
            throw new InvalidOperationException("Email already registered");
        }

        var account = new Account
        {
            Email = request.Email,
            Password = PasswordUtil.Encode(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Number = request.Number
        };
        
        var userRole = await _roleRepository.FindByName(Constants.Role.User);
        if (userRole == null)
        {
            throw new InvalidOperationException("Không tìm thấy vai trò người dùng");
        }
        account.Roles.Add(userRole);
        
        await _accountRepository.Save(account);
        await _accountRepository.SaveChanges();

        return AccountResponse.From(account, false);
    }

    public async Task<AccountResponse> Login(LoginRequest request)
    {
        var account = await _accountRepository.FindByEmail(request.Email);
        if (account == null)
        {
            throw new InvalidOperationException("Email này chưa được đăng ký");
        }

        if (!PasswordUtil.Verify(request.Password, account.Password))
        {
            throw new InvalidOperationException("Mật khẩu không đúng");
        }

        _cookieService.CreateLoginCookie(account);
        
        var response = AccountResponse.From(account, true);

        return response;
    }
}