using Microsoft.EntityFrameworkCore;
using Vanfist.Configuration.Database;
using Vanfist.Entities;

namespace Vanfist.Repositories.Impl;

public class ModelRepository : Repository<Model>, IModelRepository
{
    public ModelRepository(ApplicationDbContext context) : base(context)
    {
    }
    public async Task<IEnumerable<Model>> FindAll()
    {
        return await _context.Models
            .Include(m => m.Attachments) // load kèm ảnh
            .ToListAsync();
    }

}
