using Vanfist.DTOs.Requests;
using Vanfist.Entities;
using Vanfist.Repositories;

namespace Vanfist.Services.Impl;

public class TestDriveService : ITestDriveService
{
    private readonly ITestDriveRepository _repo;

    public TestDriveService(ITestDriveRepository repo)
    {
        _repo = repo;
    }

    public async Task<int> Create(TestDriveRequestCreate dto)
    {
        // chống spam trùng trong 24h 
        if (await _repo.ExistsRecentByContact(dto.Email?.Trim(), dto.Phone?.Trim(), TimeSpan.FromHours(24)))
            throw new InvalidOperationException("Yêu cầu trùng trong 24 giờ.");

        var entity = new TestDriveRequest
        {
            FullName = dto.FullName.Trim(),
            Email = dto.Email?.Trim(),
            Phone = dto.Phone?.Trim(),
            ModelId = dto.ModelId,
            PreferredTime = dto.PreferredTime,
            Status = "New",
            Note = dto.Note?.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        await _repo.Save(entity);
        await _repo.SaveChanges();
        return entity.Id;
    }

    public Task<TestDriveRequest?> Get(int id) => _repo.FindById(id);
}
