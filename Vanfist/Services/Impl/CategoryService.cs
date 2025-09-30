using Vanfist.Entities;
using Vanfist.Repositories;
using Vanfist.Services.Base;

namespace Vanfist.Services;

public class CategoryService : Service, ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<Category>> FindAllCategories()
    {
        return await _categoryRepository.FindAll();
    }

    public async Task<Category?> FindByIdCategory(int id)
    {
        return await _categoryRepository.FindById(id);
    }
}
