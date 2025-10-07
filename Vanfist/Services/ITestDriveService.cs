using Vanfist.DTOs.Requests;
using Vanfist.Entities;

namespace Vanfist.Services;

public interface ITestDriveService
{
    Task<int> Create(TestDriveRequestCreate dto);
    Task<TestDriveRequest?> Get(int id);
}
