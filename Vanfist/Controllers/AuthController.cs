using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vanfist.DTOs.Requests;
using Vanfist.Services;

namespace Vanfist.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
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

            return RedirectToAction("Index", "Home");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex);
            ModelState.AddModelError("", ex.Message);
            return View(request);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            ModelState.AddModelError("", "An error occurred during login. Please try again.");
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
            Console.WriteLine(ex);
            ModelState.AddModelError("", ex.Message);
            return View(request);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            ModelState.AddModelError("", "An error occurred during registration. Please try again.");
            return View(request);
        }
    }

    [Authorize(Roles = Constants.Role.UserAndAdmin)]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        try
        {
            _authService.Logout();
            return RedirectToAction("Login", "Auth");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return RedirectToAction("Index", "Account");
        }
    }
}