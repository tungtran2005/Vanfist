using Vanfist.Configuration.Database;
using Vanfist.Entities;

namespace Vanfist.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }
}
