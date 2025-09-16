using System.Security.Claims;
using Vanfist.DTOs.Responses;
using Vanfist.Entities;
using Vanfist.Repositories;
using Vanfist.Services.Base;

namespace Vanfist.Services.Impl;

public class AddressService : Service, IAddressService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAccountRepository _accountRepository;
    
    public AddressService(
        IHttpContextAccessor httpContextAccessor,
        IAccountRepository accountRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _accountRepository = accountRepository;
    }
    
    public async Task<Address> FindByDefault()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier);
        
        if (userIdClaim == null)
        {
            return null;
        }

        if (!int.TryParse(userIdClaim.Value, out int userId))
        {
            return null;
        }

        var account = await _accountRepository.FindById(userId);
        if (account == null)
        {
            return null;
        }

        foreach (var address in account.Addresses)
        {
            if (address.IsDefault)
            {
                return address;
            }
        }
        
        return null;
    }
}