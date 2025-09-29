using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vanfist.DTOs.Requests;
using Vanfist.Services;

namespace Vanfist.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountService _accountService;
    private readonly IAddressService _addressService;

    public AccountController(
        ILogger<AccountController> logger,
        IAccountService accountService,
        IAddressService addressService)
    {
        _logger = logger;
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
            
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching account information");
            TempData["ErrorMessage"] = "Đã xảy ra lỗi khi tải thông tin tài khoản.";
            return RedirectToAction("Index", "Home");
        }
    }
    
    [Authorize(Roles = Constants.Role.UserAndAdmin)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateInformation(UpdateAccountRequest request)
    {
        try
        {
            await _accountService.UpdateInformation(request);
            TempData["SuccessMessage"] = "Cập nhật thông tin thành công.";
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error updating account information");
            TempData["ErrorMessage"] = "Cập nhật thông tin thất bại.";
        }

        return RedirectToAction("Index");
    }

    [Authorize(Roles = Constants.Role.UserAndAdmin)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        try
        {
             await _accountService.ChangePassword(request);
            TempData["SuccessMessage"] = "Đổi mật khẩu thành công.";
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error changing password");
            TempData["ErrorMessage"] = "Đổi mật khẩu thất bại.";
            throw;
        }

        return RedirectToAction("Index");
    }
}