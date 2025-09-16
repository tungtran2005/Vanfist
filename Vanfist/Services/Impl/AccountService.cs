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

    public AccountService(
        IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
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