using Vanfist.Configuration.Database;

namespace Vanfist.Services.Base;

public class Service : IService
{
    protected readonly ApplicationDbContext _context;
    
    public Service(ApplicationDbContext context)
    {
        _context = context;
    }
}