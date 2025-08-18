using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Vanfist.Constants;
using Vanfist.Models;
using Vanfist.Services;

namespace Vanfist.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAccountService _accountService;

    public HomeController(ILogger<HomeController> logger,
        IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }

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

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}