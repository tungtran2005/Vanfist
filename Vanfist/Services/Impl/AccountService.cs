using System.Security.Claims;
using Vanfist.DTOs.Responses;
using Vanfist.Repositories;
using Vanfist.Services.Base;
using Microsoft.EntityFrameworkCore;
using Vanfist.Configuration.Database;
using Vanfist.DTOs.Requests;
using Vanfist.Entities;
using Vanfist.Utils;

namespace Vanfist.Services.Impl;

public class AccountService : Service, IAccountService
{
    private readonly IAccountRepository _accountRepository;   
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountService(
        IAccountRepository accountRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _accountRepository = accountRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AccountResponse> getCurrentAccount()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return AccountResponse.From(null, false);
        }

        if (!int.TryParse(userIdClaim.Value, out int userId))
        {
            return AccountResponse.From(null, false);
        }

        var account = await _accountRepository.FindById(userId);
        if (account == null)
        {
            return AccountResponse.From(null, false);
        }

        return AccountResponse.From(account, true);
    }

    public async Task<AccountResponse> FindById(int id)
    {
        
        var account = await _accountRepository.FindById(id);
        
        if (account == null)
        {
            throw new InvalidOperationException($"Account with ID {id} not found");
        }
        
        var response = AccountResponse.From(account, true);
        
        return response;
    }
}