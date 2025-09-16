using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vanfist.Constants;
using Vanfist.Services;

namespace Vanfist.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountService _accountService;

    public AccountController(ILogger<AccountController> logger,
        IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }

    [Authorize(Roles = Constants.Role.UserAndAdmin)]
    [HttpGet]
    public async Task<IActionResult> Index()
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
            _logger.LogError(ex, "Lỗi khi load profile");
            ViewBag.IsLoggedIn = false;
            ViewBag.Account = null;
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
            _logger.LogError(ex, "Lỗi khi load profile");
            ViewBag.IsLoggedIn = false;
            ViewBag.Account = null;
        }

        return View();
    }
}