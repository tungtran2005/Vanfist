using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vanfist.Entities;
using Vanfist.Repositories;

namespace Vanfist.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AdminController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Index()
        {
            var admin = new Account
            {
                Email = "admin@vanfist.com",
                FirstName = "Admin",
                LastName = "Vanfist"
                
            };

            ViewBag.Admin = admin;
            return View();
        }
    }
}
