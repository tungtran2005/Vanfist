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
    private readonly IAddressRepository _addressRepository;
    
    public AddressService(
        IHttpContextAccessor httpContextAccessor,
        IAccountRepository accountRepository,
        IAddressRepository addressRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _accountRepository = accountRepository;
        _addressRepository = addressRepository;
    }

    public async Task<Address> GetDefaultAddress()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            throw new Exception("Không tìm thấy userId trong claim");
        }

        if (!int.TryParse(userIdClaim.Value, out int userId))
        {
            throw new Exception("UserId trong claim không hợp lệ");
        }

        var account = await _accountRepository.FindById(userId);
        if (account == null)
        {
            throw new Exception($"Không tìm thấy tài khoản với ID {userId}");
        }

        var address = await _addressRepository.FindByDefaultAndAccountId(account.Id);
        if (address == null)
        {
            throw new Exception("Không tìm thấy địa chỉ mặc định cho tài khoản này");
        }
        return address;
    }
}