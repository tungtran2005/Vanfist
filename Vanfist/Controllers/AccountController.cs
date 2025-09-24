using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vanfist.DTOs.Requests;
using Vanfist.Services;

namespace Vanfist.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IAddressService _addressService;

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
            var account = await _accountService.GetCurrentAccount();
            ViewBag.Account = account;
            
            var defaultAddress = await _addressService.GetDefaultAddress();
            ViewBag.DefaultAddress = defaultAddress;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return RedirectToAction("Index", "Home");
        }

        return View();
    }
    
    [Authorize(Roles = Constants.Role.UserAndAdmin)]
    [HttpPost]
    public async Task<IActionResult> UpdateInformation(UpdateAccountRequest request)
    {
        try
        {
            var account = await _accountService.UpdateInformation(request);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return RedirectToAction("Index");
        }

        return RedirectToAction("Index");
    }

    [Authorize(Roles = Constants.Role.UserAndAdmin)]
    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        try
        {
             await _accountService.ChangePassword(request);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return RedirectToAction("Index");
    }
}