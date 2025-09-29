using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Vanfist.DTOs.Responses;
using Vanfist.Repositories;
using Vanfist.Services.Base;
using Vanfist.DTOs.Requests;
using Vanfist.Entities;

namespace Vanfist.Services.Impl;

public class AccountService : Service, IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAddressRepository _addressRepository;
    private readonly IPasswordService _passwordService;

    public AccountService(
        IAccountRepository accountRepository,
        IHttpContextAccessor httpContextAccessor,
        IAddressRepository addressRepository,
        IPasswordService passwordService)
    {
        _accountRepository = accountRepository;
        _httpContextAccessor = httpContextAccessor;
        _addressRepository = addressRepository;
        _passwordService = passwordService;
    }

    public async Task<AccountResponse> GetCurrentAccount()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return AccountResponse.From(null);
        }

        if (!int.TryParse(userIdClaim.Value, out int userId))
        {
            return AccountResponse.From(null);
        }

        var account = await _accountRepository.FindById(userId);
        if (account == null)
        {
            return AccountResponse.From(null);
        }

        return AccountResponse.From(account);
    }

    public async Task<AccountResponse> FindById(int id)
    {
        var account = await _accountRepository.FindById(id);

        if (account == null)
        {
            throw new InvalidOperationException($"Không tìm thấy tài khoản với ID {id}");
        }

        var response = AccountResponse.From(account);

        return response;
    }
    
    public async Task<AccountResponse> UpdateInformation(UpdateAccountRequest request)
    {
        var account = await GetAccountFromContextAsync();

        // Chuẩn hóa dữ liệu đầu vào
        account.FirstName = request.FirstName?.Trim();
        account.LastName = request.LastName?.Trim();
        account.Number = string.IsNullOrWhiteSpace(request.Number) ? null : request.Number.Trim();

        var defaultAddress = await _addressRepository.FindByDefaultAndAccountId(account.Id);

        var detail = request.Detail?.Trim();
        var city = request.City?.Trim();

        if (defaultAddress == null)
        {
            defaultAddress = new Address
            {
                AccountId = account.Id,
                Detail = detail,
                City = city,
                IsDefault = true
            };
            await _addressRepository.Save(defaultAddress);
            await _addressRepository.SaveChanges();
        }
        else
        {
            defaultAddress.Detail = detail;
            defaultAddress.City = city;
            await _addressRepository.Update(defaultAddress);
            await _addressRepository.SaveChanges();
        }

        await _accountRepository.Update(account);
        await _accountRepository.SaveChanges();

        return AccountResponse.From(account);
    }

    public async Task ChangePassword(ChangePasswordRequest request)
    {
        var account = await GetAccountFromContextAsync();

        var isVerified = _passwordService.Verify(account.Password, request.OldPassword);
        if (!isVerified)
        {
            throw new InvalidOperationException("Mật khẩu cũ không đúng");
        }

        account.Password = _passwordService.Encode(request.NewPassword);

        await _accountRepository.Update(account);
        await _accountRepository.SaveChanges();
    }


    private async Task<Account> GetAccountFromContextAsync()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            throw new InvalidOperationException("Người dùng chưa được xác thực");
        }

        if (!int.TryParse(userIdClaim.Value, out int userId))
        {
            throw new InvalidOperationException("Invalid user ID");
        }

        var account = await _accountRepository.FindById(userId);
        if (account == null)
        {
            throw new InvalidOperationException($"Không tìm thấy tài khoản với ID {userId}");
        }

        return account;
    }
}