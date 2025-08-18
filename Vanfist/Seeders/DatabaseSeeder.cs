using Vanfist.Entities;
using Vanfist.Repositories;

namespace Vanfist.Seeders;

public class DatabaseSeeder
{
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(ILogger<DatabaseSeeder> logger)
    {
        _logger = logger; 
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("DatabaseSeeder: Start seeding");

        _logger.LogInformation("DatabaseSeeder: Seeding completed");
    }
}


