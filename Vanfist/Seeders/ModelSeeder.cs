using Microsoft.Extensions.Logging;
using Vanfist.Entities;
using Vanfist.Repositories;

namespace Vanfist.Seeders
{
    public class ModelSeeder
    {
        private readonly ILogger<ModelSeeder> _logger;
        private readonly IModelRepository _modelRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ModelSeeder(
            ILogger<ModelSeeder> logger,
            IModelRepository modelRepository,
            ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _modelRepository = modelRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task Seed()
        {
            _logger.LogInformation("ModelSeeder: Start seeding models");

            var existing = await _modelRepository.FindAll();
            if (existing.Any())
            {
                _logger.LogInformation("Models already exist, skipping seeding");
                return;
            }

            // Lấy category đã seed để dùng Id
            var categories = (await _categoryRepository.FindAll()).ToList();
            if (!categories.Any())
            {
                _logger.LogWarning("No categories found in DB, cannot seed models");
                return;
            }

            // Bảng spec mẫu cho mỗi dòng (chỉ 1 mẫu mỗi dòng), bạn có thể thêm hoặc sửa số liệu thực tế
            var baseSpecs = new List<(string name, float price, float length, float width, float height, float wheelbase, float nedc, float maxPower, float maxTorque, float rimSize)>
            {
                ("VF 3", 400000000f, 3190f, 1679f, 1622f, 2075f, 210f, 32f, 90f, 15f),
                ("VF 5", 600000000f, 3967f, 1723f, 1578f, 2514f, 260f, 94f, 135f, 16f),
                ("VF 6", 700000000f, 4320f, 1790f, 1585f, 2600f, 300f, 120f, 200f, 17f),
                ("VF 7", 900000000f, 4545f, 1890f, 1636f, 2840f, 350f, 150f, 250f, 18f),
                ("VF 9", 1500000000f, 5070f, 1990f, 1720f, 3000f, 400f, 200f, 320f, 19f),
            };

            // Các màu để biến thể
            var colors = new string[] { "Đỏ", "Xanh", "Trắng", "Đen", "Xám", "Vàng" };

            foreach (var spec in baseSpecs)
            {
                // Tìm category Id theo tên giống
                var cat = categories.FirstOrDefault(c => c.Name == spec.name);
                if (cat == null)
                {
                    _logger.LogWarning("Category {Name} not found, skipping model {Name}", spec.name, spec.name);
                    continue;
                }

                // Tạo biến thể cho mỗi màu
                foreach (var color in colors)
                {
                    var model = new Model
                    {
                        Name = spec.name,
                        Price = spec.price,
                        Length = spec.length,
                        Width = spec.width,
                        Height = spec.height,
                        Wheelbase = spec.wheelbase,
                        NEDC = spec.nedc,
                        MaximumPower = spec.maxPower,
                        MaximumTorque = spec.maxTorque,
                        RimSize = spec.rimSize,
                        Color = color,
                        CategoryId = cat.Id
                    };

                    await _modelRepository.Save(model);
                    _logger.LogInformation("Seeded model: {Name}, color {Color}", spec.name, color);
                }
            }

            await _modelRepository.SaveChanges();
            _logger.LogInformation("ModelSeeder: Seeding completed");
        }
    }
}
