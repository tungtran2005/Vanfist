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

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.Session.GetInt32(Session.AccountId);
        var isLoggedIn = userId.HasValue;
        ViewBag.IsLoggedIn = isLoggedIn;

        if (isLoggedIn)
        {
            try
            {
                ViewBag.Account = await _accountService.FindById(userId.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load account for user id {userId}", userId);
                ViewBag.Account = null;
                ViewBag.IsLoggedIn = false;
            }
        }
        else
        {
            ViewBag.Account = null;
        }

        return View();
    }
}