using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vanfist.Entities;
using Vanfist.Repositories;

namespace Vanfist.Controllers
{
    [Authorize(Roles = Constants.Role.Admin)]
    public class AdminController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AdminController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Index()
        {
            var admin = await _accountRepository.FindByEmail("admin@vanfist.com");
            if (admin == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            ViewBag.Admin = admin;
            return View();
        }
    }
}
