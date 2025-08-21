using Vanfist.Entities;
using Vanfist.Repositories;

namespace Vanfist.Seeders;

public class DatabaseSeeder
{
    private readonly ILogger<DatabaseSeeder> _logger;
    private readonly RoleSeeder _roleSeeder;

    public DatabaseSeeder(ILogger<DatabaseSeeder> logger,
        RoleSeeder roleSeeder)
    {
        _logger = logger;
        _roleSeeder = roleSeeder;
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("DatabaseSeeder: Start seeding");

        await _roleSeeder.Seed();

        _logger.LogInformation("DatabaseSeeder: Seeding completed");
    }
}


