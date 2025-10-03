using Microsoft.Extensions.Logging;
using Vanfist.Entities;
using Vanfist.Repositories;

namespace Vanfist.Seeders
{
    public class CategorySeeder
    {
        private readonly ILogger<CategorySeeder> _logger;
        private readonly ICategoryRepository _categoryRepository;

        public CategorySeeder(
            ILogger<CategorySeeder> logger,
            ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        public async Task Seed()
        {
            _logger.LogDebug("(Seed) Seeding categories");

            // Nếu đã có category thì bỏ qua
            var existingCategories = await _categoryRepository.FindAll();
            if (existingCategories.Any())
            {
                _logger.LogInformation("Categories already exist, skipping seeding.");
                return;
            }

            var categories = new List<Category>
            {
                new Category { Name = "VF 3", Description = "Dòng xe VF 3" },
                new Category { Name = "VF 5", Description = "Dòng xe VF 5" },
                new Category { Name = "VF 6", Description = "Dòng xe VF 6" },
                new Category { Name = "VF 7", Description = "Dòng xe VF 7" },
                new Category { Name = "VF 9", Description = "Dòng xe VF 9" }
            };


            foreach (var category in categories)
            {
                await _categoryRepository.Save(category);
                _logger.LogInformation("Seeded category: {Category}", category.Name);
            }

            await _categoryRepository.SaveChanges();
            _logger.LogInformation("Category seeding completed");
        }
    }
}
