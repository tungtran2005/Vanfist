using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vanfist.DTOs.Requests;
using Vanfist.Services;

namespace Vanfist.Controllers;

public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;

    public AuthController(
        ILogger<AuthController> logger,
        IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            
            return View(request);
        }

        try
        {
            var response = await _authService.Login(request);
            TempData["SuccessMessage"] = "Đăng nhập thành công.";

            return RedirectToAction("Index", "Home");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error logging in");
            ModelState.AddModelError("", ex.Message);
            TempData["ErrorMessage"] = "Đăng nhập thất bại.";
            return View(request);
        }
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        try
        {
            var response = await _authService.Register(request);

            TempData["SuccessMessage"] = "Đăng ký thành công! Mời quý khách đăng nhập.";
            return RedirectToAction("Login");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error registering");
            ModelState.AddModelError("", ex.Message);
            TempData["ErrorMessage"] = "Đăng ký thất bại.";
            return View(request);
        }
    }

    [Authorize(Roles = Constants.Role.UserAndAdmin)]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        _authService.Logout();
        TempData["SuccessMessage"] = "Đăng xuất thành công.";
        return RedirectToAction("Login", "Auth");
    }
}