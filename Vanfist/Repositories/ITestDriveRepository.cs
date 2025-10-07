using Vanfist.Entities;

namespace Vanfist.Repositories;

public interface ITestDriveRepository : IRepository<TestDriveRequest>
{
    Task<bool> ExistsRecentByContact(string? email, string? phone, TimeSpan within);
}
