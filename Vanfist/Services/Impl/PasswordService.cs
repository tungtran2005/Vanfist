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
    
    public string Encode(string password)
    {
        return password;
    }

    public bool Verify(string hashedPassword, string password)
    {
        return hashedPassword == password;
    }
}