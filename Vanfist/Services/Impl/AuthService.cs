using Vanfist.Entities;
using Vanfist.Repositories;
using Vanfist.DTOs.Responses;
using LoginRequest = Vanfist.DTOs.Requests.LoginRequest;
using RegisterRequest = Vanfist.DTOs.Requests.RegisterRequest;
using Vanfist.Services.Base;

namespace Vanfist.Services.Impl;

public class AuthService : Service, IAuthService
{
    private readonly ICookieService _cookieService;
    private readonly IPasswordService _passwordService;
    private readonly IAccountRepository _accountRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IAddressRepository _addressRepository;

    public AuthService(
        ICookieService cookieService,
        IAccountRepository accountRepository,
        IRoleRepository roleRepository,
        IAddressRepository addressRepository,
        IPasswordService passwordService)
    {
        _cookieService = cookieService;
        _accountRepository = accountRepository;
        _roleRepository = roleRepository;
        _passwordService = passwordService;
        _addressRepository = addressRepository;
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
            FirstName = request.FirstName,
            LastName = request.LastName,
            Number = request.Number
        };
        
        var hashedPassword = _passwordService.Encode(account, request.Password);
        account.Password = hashedPassword;
        
        var userRole = await _roleRepository.FindByName(Constants.Role.User);
        if (userRole == null)
        {
            throw new InvalidOperationException("Không tìm thấy vai trò người dùng");
        }
        account.Roles.Add(userRole);
        
        var address = new Address
        {
            Detail = null,
            City = null,
            IsDefault = true,
            Account = account
        };
        
        await _accountRepository.Save(account);
        await _accountRepository.SaveChanges();
        await _addressRepository.Save(address);
        await _addressRepository.SaveChanges();

        return AccountResponse.From(account);
    }

    public async Task<AccountResponse> Login(LoginRequest request)
    {
        var account = await _accountRepository.FindByEmail(request.Email);
        if (account == null)
        {
            throw new InvalidOperationException("Email này chưa được đăng ký");
        }

        if (!_passwordService.Verify(account, account.Password, request.Password))
        {
            throw new InvalidOperationException("Mật khẩu không đúng");
        }

        _cookieService.CreateLoginCookie(account);
        
        var response = AccountResponse.From(account);

        return response;
    }
}