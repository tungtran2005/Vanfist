using Vanfist.Configuration;
using Vanfist.DTOs.Responses;
using Vanfist.Entities;
using Vanfist.Repositories;
using Vanfist.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Vanfist.Services.Impl;

public class AccountService : Service, IAccountService
{
    private readonly ILogger<AccountService> _logger;
    private readonly IAccountRepository _accountRepository;

    public AccountService(ILogger<AccountService> logger,
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
        
        var account = await _context.Accounts
            .FirstOrDefaultAsync(a => a.Id == id);
        
        if (account == null)
        {
            throw new InvalidOperationException($"Account with ID {id} not found");
        }
        
        var response = AccountResponse.From(account);
        
        _logger.LogInformation("(FindById) Account found successfully. Account: {account}", response);
        
        return response;
    }
}