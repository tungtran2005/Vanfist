using Microsoft.EntityFrameworkCore;
using Vanfist.Configuration.Database;
using Vanfist.Entities;

namespace Vanfist.Repositories.Impl;

public class TestDriveRepository : Repository<TestDriveRequest>, ITestDriveRepository
{
    public TestDriveRepository(ApplicationDbContext context) : base(context) { }

    public async Task<bool> ExistsRecentByContact(string? email, string? phone, TimeSpan within)
    {
        var since = DateTime.UtcNow - within;
        return await _context.TestDriveRequests
            .AnyAsync(x => x.CreatedAt >= since &&
                           ((email != null && x.Email == email) ||
                            (phone != null && x.Phone == phone)));
    }
}
