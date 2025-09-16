using Microsoft.AspNetCore.Identity;
using Vanfist.Entities;

namespace Vanfist.Services.Impl;

public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<Account> _passwordHasher;
    
    public PasswordService()
    {
        _passwordHasher = new PasswordHasher<Account>();
    }
    
    public string Encode(Account account, string password)
    {
        return _passwordHasher.HashPassword(account, password);
    }

    public bool Verify(Account account, string hashedPassword, string password)
    {
        return _passwordHasher.VerifyHashedPassword(account, hashedPassword, password) == PasswordVerificationResult.Success;
    }
}