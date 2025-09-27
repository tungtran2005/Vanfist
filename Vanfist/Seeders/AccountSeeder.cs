using Microsoft.Extensions.Logging;
using Vanfist.Entities;
using Vanfist.Repositories;
using Vanfist.Services;

namespace Vanfist.Seeders
{
    public class AccountSeeder
    {
        private readonly ILogger<AccountSeeder> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordService _passwordService;

        public AccountSeeder(
            ILogger<AccountSeeder> logger,
            IAccountRepository accountRepository,
            IRoleRepository roleRepository,
            IPasswordService passwordService)
        {
            _logger = logger;
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _passwordService = passwordService;
        }

        public async Task Seed()
        {
            _logger.LogDebug("(Seed) Seeding admin account");

            var adminEmail = "admin@vanfist.com";
            var adminPassword = "Admin@123";

            var existingAdmin = await _accountRepository.FindByEmail(adminEmail);
            if (existingAdmin != null)
            {
                _logger.LogInformation("Admin account already exists: {Email}", adminEmail);
                return;
            }

            var adminRole = await _roleRepository.FindByName("Admin");
            if (adminRole == null)
            {
                _logger.LogError("Role 'Admin' not found. Seed RoleSeeder first!");
                return;
            }

            var adminAccount = new Account
            {
                Email = adminEmail,
                Password = _passwordService.Encode(adminPassword),
                FirstName = "System",
                LastName = "Admin",
                Roles = new List<Role> { adminRole }
            };

            await _accountRepository.Save(adminAccount);
            await _accountRepository.SaveChanges();

            _logger.LogInformation("Admin account seeded: {Email}", adminEmail);
        }
    }
}
