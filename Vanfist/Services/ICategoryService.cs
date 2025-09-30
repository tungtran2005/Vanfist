using Vanfist.Entities;
using Vanfist.Services.Base;

namespace Vanfist.Services;

public interface ICategoryService : IService
{
    Task<IEnumerable<Category>> FindAllCategories();
    Task<Category?> FindByIdCategory(int id);
}
