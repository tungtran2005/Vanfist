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
            .Include(m => m.Category)    // thêm dòng này
            .Include(m => m.Attachments) // load ảnh
            .ToListAsync();
    }

    public async Task<Model?> FindById(int id)
    {
        return await _context.Models
            .Include(m => m.Category)    // load Category
            .Include(m => m.Attachments) // load ảnh
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Model>> FindByCategoriesId(List<int> categoryIds)
    {
        var query = _context.Models
            .Include(m => m.Category)
            .Include(m => m.Attachments)   // <-- thêm dòng này
            .AsQueryable();

        if (categoryIds != null && categoryIds.Any())
        {
            query = query.Where(m => categoryIds.Contains(m.CategoryId));
        }

        return await query.ToListAsync();
    }




}
