using Vanfist.Entities;
using Vanfist.Repositories;

namespace Vanfist.Seeders;

public class DatabaseSeeder
{
    private readonly ILogger<DatabaseSeeder> _logger;
    private readonly RoleSeeder _roleSeeder;
    private readonly AccountSeeder _accountSeeder;

    public DatabaseSeeder(
        ILogger<DatabaseSeeder> logger,
        RoleSeeder roleSeeder,
        AccountSeeder accountSeeder)
    {
        _logger = logger;
        _roleSeeder = roleSeeder;
        _accountSeeder = accountSeeder;
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("DatabaseSeeder: Start seeding");

        await _roleSeeder.Seed();
        await _accountSeeder.Seed();

        _logger.LogInformation("DatabaseSeeder: Seeding completed");
    }
}



