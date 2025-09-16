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

    public AccountService(
        IAccountRepository accountRepository,
        IHttpContextAccessor httpContextAccessor,
        IAddressRepository addressRepository)
    {
        _accountRepository = accountRepository;
        _httpContextAccessor = httpContextAccessor;
        _addressRepository = addressRepository;
    }

    public async Task<AccountResponse> getCurrentAccount()
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
            throw new InvalidOperationException($"Account with ID {id} not found");
        }

        var response = AccountResponse.From(account);

        return response;
    }
    
    public async Task<AccountResponse> UpdateInformation(UpdateAccountRequest request)
    {
        var account = await GetAccountFromContextAsync();

        account.FirstName = request.FirstName;
        account.LastName = request.LastName;

        var defaultAddress = await _addressRepository.FindByDefaultAndAccountId(account.Id);

        if (defaultAddress == null)
        {
            // Create a new default address if none exists
            defaultAddress = new Address
            {
                AccountId = account.Id,
                Detail = request.Detail,
                City = request.City,
                IsDefault = true
            };
            await _addressRepository.Save(defaultAddress);
            await _addressRepository.SaveChanges();
        }
        else
        {
            defaultAddress.Detail = request.Detail;
            defaultAddress.City = request.City;

            await _addressRepository.SaveChanges();
        }

        await _accountRepository.SaveChanges();

        return AccountResponse.From(account);
    }

    public async void ChangePassword(ChangePasswordRequest request)
    {
        var account = await GetAccountFromContextAsync();

        var hasher = new PasswordHasher<Account>();

        var result = hasher.VerifyHashedPassword(account, account.Password, request.OldPassword);
        if (result == PasswordVerificationResult.Failed)
        {
            throw new InvalidOperationException("Mật khẩu cũ không đúng");
        }

        account.Password = hasher.HashPassword(account, request.NewPassword);

        await _accountRepository.SaveChanges();
    }


    private async Task<Account> GetAccountFromContextAsync()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            throw new InvalidOperationException("User is not authenticated");
        }

        if (!int.TryParse(userIdClaim.Value, out int userId))
        {
            throw new InvalidOperationException("Invalid user ID");
        }

        var account = await _accountRepository.FindById(userId);
        if (account == null)
        {
            throw new InvalidOperationException($"Account with ID {userId} not found");
        }

        return account;
    }
}