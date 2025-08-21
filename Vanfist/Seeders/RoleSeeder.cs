using Vanfist.Entities;
using Vanfist.Repositories;

namespace Vanfist.Seeders;

public class RoleSeeder
{
    private readonly ILogger<RoleSeeder> _logger;
    private readonly IRoleRepository _roleRepository;

    public RoleSeeder(ILogger<RoleSeeder> logger,
        IRoleRepository roleRepository)
    {
        _logger = logger;
        _roleRepository = roleRepository;
    }

    public async Task Seed()
    {
        _logger.LogDebug("(Seed) Seeding roles");
        var roleNames = Constants.Role.List();

        foreach (var name in roleNames)
        {
            if (!await _roleRepository.ExistsByName(name))
            {
                var role = new Role()
                {
                    Name = name,
                    Description = "Đây là vai trò " + name
                };
                await _roleRepository.Save(role);
            }
        }

        await _roleRepository.SaveChanges();
    }
}