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
    private readonly ILogger<AccountService> _logger;
    private readonly IAccountRepository _accountRepository;

    public AccountService(
        ILogger<AccountService> logger,
        IAccountRepository accountRepository,
        ApplicationDbContext context)
        : base(context)
    {
        _logger = logger;
        _accountRepository = accountRepository;
    }

    public async Task<AccountResponse> FindById(int id)
    {
        _logger.LogInformation("(FindById) id: {id}", id);
        
        var account = await _accountRepository.FindById(id);
        
        if (account == null)
        {
            throw new InvalidOperationException($"Account with ID {id} not found");
        }
        
        var response = AccountResponse.From(account);
        
        _logger.LogInformation("(FindById) Account found successfully. Account: {account}", response);
        
        return response;
    }
}