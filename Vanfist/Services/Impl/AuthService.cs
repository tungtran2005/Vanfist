using Vanfist.Entities;
using Vanfist.Repositories;
using Vanfist.DTOs.Responses;
using Vanfist.Configuration;
using LoginRequest = Vanfist.DTOs.Requests.LoginRequest;
using RegisterRequest = Vanfist.DTOs.Requests.RegisterRequest;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Vanfist.Constants;
using Vanfist.Services.Base;

namespace Vanfist.Services.Impl;

public class AuthService : Service, IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAccountRepository _accountRepository;

    public AuthService(ILogger<AuthService> logger,
        IHttpContextAccessor httpContextAccessor,
        IAccountRepository accountRepository,
        ApplicationDbContext context) 
        : base(context)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _accountRepository = accountRepository;
    }

    public async Task<AccountResponse> Register(RegisterRequest request)
    {
        _logger.LogInformation("(Register) request: {request}", request);

        _logger.LogDebug("(Register) check if email already exists");
        var existingAccount = await _context.Accounts
            .FirstOrDefaultAsync(a => a.Email == request.Email);

        if (existingAccount != null)
        {
            throw new InvalidOperationException("Email already registered");
        }

        _logger.LogDebug("(Register) create new account");
        var account = new Account
        {
            Email = request.Email,
            Password = Encode(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Number = request.Number
        };

        await _accountRepository.AddAsync(account);
        await _accountRepository.SaveChangesAsync();

        _logger.LogInformation("(Register) create account successfully. Account: {account}", account);

        return AccountResponse.From(account);
    }

    public async Task<AccountResponse> Login(LoginRequest request)
    {
        _logger.LogInformation("(Login) request: {request}", request);

        _logger.LogDebug("(Login) find account by email");
        var account = await _context.Accounts
            .FirstOrDefaultAsync(a => a.Email == request.Email);

        if (account == null)
        {
            throw new InvalidOperationException("Email not found");
        }

        _logger.LogDebug("(Login) verify password");
        if (!VerifyPassword(request.Password, account.Password))
        {
            throw new InvalidOperationException("Password is incorrect");
        }

        _logger.LogDebug("(Login) create session");
        var context = _httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("HttpContext is null");
        
        context.Session.SetInt32(Session.AccountId, account.Id);

        var response = AccountResponse.From(account);
        _logger.LogInformation("(Login) User logged in successfully. Response: {response}", response);

        return response;
    }

    private string Encode(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        var hashedInput = Encode(password);
        return hashedInput == hashedPassword;
    }
}