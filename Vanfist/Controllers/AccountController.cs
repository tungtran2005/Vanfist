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

            // Populate the view model so the form is prefilled
            var model = new UpdateAccountRequest
            {
                FirstName = account.FirstName,
                LastName = account.LastName,
                Number = account.Number,
                Detail = defaultAddress?.Detail,
                City = defaultAddress?.City
            };

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching account information");
            return RedirectToAction("Index", "Home");
        }
    }
    
    [Authorize(Roles = Constants.Role.UserAndAdmin)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateInformation(UpdateAccountRequest request)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Cập nhật thông tin thất bại.");

            // Reload ViewBag data needed by Index and return the same request model so validation messages show
            try
            {
                var account = await _accountService.GetCurrentAccount();
                ViewBag.Account = account;

                var defaultAddress = await _addressService.GetDefaultAddress();
                ViewBag.DefaultAddress = defaultAddress;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reloading data after validation failure");
            }

            return View("Index", request);
        }

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