using Vanfist.Entities;
using Vanfist.Repositories;

namespace Vanfist.Seeders;

public class DatabaseSeeder
{
    private readonly ILogger<DatabaseSeeder> _logger;
    private readonly RoleSeeder _roleSeeder;
    private readonly AccountSeeder _accountSeeder;
    private readonly CategorySeeder _categorySeeder;
    private readonly ModelSeeder _modelSeeder;

    public DatabaseSeeder(
        ILogger<DatabaseSeeder> logger,
        RoleSeeder roleSeeder,
        AccountSeeder accountSeeder,
        CategorySeeder categorySeeder,
        ModelSeeder modelSeeder)
    {
        _logger = logger;
        _roleSeeder = roleSeeder;
        _accountSeeder = accountSeeder;
        _categorySeeder = categorySeeder;
        _modelSeeder = modelSeeder;
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("DatabaseSeeder: Start seeding");

        await _roleSeeder.Seed();
        await _accountSeeder.Seed();
        await _categorySeeder.Seed();
        await _modelSeeder.Seed();

        _logger.LogInformation("DatabaseSeeder: Seeding completed");
    }
}



