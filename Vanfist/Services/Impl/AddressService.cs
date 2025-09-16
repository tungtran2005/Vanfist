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
}