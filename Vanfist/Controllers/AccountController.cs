using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vanfist.DTOs.Responses;
using Vanfist.Repositories;
using Vanfist.Services;

namespace Vanfist.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly  IAddressService _addressService;

    public AccountController(
        IAccountService accountService,
        IAddressService addressService)
    {
        _accountService = accountService;
        _addressService = addressService;
    }

    [Authorize(Roles = Constants.Role.UserAndAdmin)]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var account = await _accountService.getCurrentAccount();
            ViewBag.Account = account;
            
            var defaultAddress = await _addressService.FindByDefault();
            ViewBag.DefaultAddress = defaultAddress;
        }
        catch (Exception ex)
        {
            ViewBag.Account = AccountResponse.From(null, false);
        }

        return View();
    }
    
    [Authorize(Roles = Constants.Role.UserAndAdmin)]
    [HttpGet]
    public async Task<IActionResult> UpdateInformation()
    {
        try
        {
            var userIdClaim = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            int userId = int.Parse(userIdClaim.Value);

            var account = await _accountService.FindById(userId);

            ViewBag.IsLoggedIn = true;
            ViewBag.Account = account;
        }
        catch (Exception ex)
        {
            ViewBag.IsLoggedIn = false;
            ViewBag.Account = null;
        }

        return View();
    }
}