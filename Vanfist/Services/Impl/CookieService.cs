using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Vanfist.Entities;
using Vanfist.Services.Base;

namespace Vanfist.Services.Impl;

public class CookieService : Service, ICookieService
{
    
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public CookieService(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public void CreateLoginCookie(Account account)
    {
        var context = _httpContextAccessor.HttpContext
                      ?? throw new InvalidOperationException("HttpContext is null");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString())
        };
        
        foreach (var role in account.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
        };

        context.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties
        ).GetAwaiter().GetResult();
    }
}