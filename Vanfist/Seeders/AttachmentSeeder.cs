using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using Vanfist.Repositories;
using Vanfist.Services;

namespace Vanfist.Seeders;

public class AttachmentSeeder
{
    private readonly ILogger<AttachmentSeeder> _logger;
    private readonly IModelRepository _modelRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IAttachmentService _attachmentService;
    private readonly IWebHostEnvironment _env;

    public AttachmentSeeder(
        ILogger<AttachmentSeeder> logger,
        IModelRepository modelRepository,
        ICategoryRepository categoryRepository,
        IAttachmentService attachmentService,
        IWebHostEnvironment env)
    {
        _logger = logger;
        _modelRepository = modelRepository;
        _categoryRepository = categoryRepository;
        _attachmentService = attachmentService;
        _env = env;
    }

    public async Task Seed()
    {
        _logger.LogInformation("AttachmentSeeder: Start seeding attachments");

        var models = await _modelRepository.FindAll();
        var categories = (await _categoryRepository.FindAll()).ToDictionary(c => c.Id, c => c.Name);

        var webRoot = _env.WebRootPath ?? "wwwroot";
        var seedRoot = Path.Combine(webRoot, "seed");

        if (!Directory.Exists(seedRoot))
        {
            _logger.LogWarning("Seed folder not found: {SeedRoot}. Skipping attachment seeding.", seedRoot);
            return;
        }

        foreach (var m in models)
        {
            var categoryName = categories.TryGetValue(m.CategoryId, out var name) ? name : null;
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                _logger.LogWarning("Model {Id} has no valid category, skip seeding images.", m.Id);
                continue;
            }


            var existing = await _attachmentService.ListByModel(m.Id);
            if (existing != null && existing.Any())
            {
                _logger.LogDebug("Model {Id} already has {Count} attachments. Skipping.", m.Id, existing.Count());
                continue;
            }

            var sourceDir = Path.Combine(seedRoot, SafeSegment(categoryName), SafeSegment(m.Color ?? string.Empty));
            if (!Directory.Exists(sourceDir))
            {
                _logger.LogWarning("Source folder not found for model {Id}: {Dir}", m.Id, sourceDir);
                continue;
            }

            var sourceFiles = Directory.EnumerateFiles(sourceDir)
                                       .Where(f => HasImageExtension(f))
                                       .OrderBy(f => f, StringComparer.OrdinalIgnoreCase)
                                       .Take(2)
                                       .ToList();

            if (sourceFiles.Count < 2)
            {
                _logger.LogWarning("Model {Id}: need 2 images, found {Count} in {Dir}", m.Id, sourceFiles.Count, sourceDir);
                continue;
            }

            int uploaded = 0;
            foreach (var path in sourceFiles)
            {
                // B?c file ngu?n thành IFormFile ?? tái s? d?ng logic UploadAsync
                using var stream = System.IO.File.OpenRead(path);
                var formFile = new FormFile(stream, 0, stream.Length, "file", Path.GetFileName(path))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = GetContentType(path)
                };

                await _attachmentService.UploadAsync(formFile, m.Id);
                uploaded++;
            }

            _logger.LogInformation("Model {Id}: seeded {Uploaded}/2 attachments from {Dir}", m.Id, uploaded, sourceDir);
        }

        _logger.LogInformation("AttachmentSeeder: Completed");
    }

    private static string SafeSegment(string segment)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var cleaned = new string(segment.Select(ch => invalid.Contains(ch) ? '_' : ch).ToArray());
        return cleaned.Trim();
    }

    private static bool HasImageExtension(string filePath)
    {
        var ext = Path.GetExtension(filePath).ToLowerInvariant();
        return ext is ".jpg" or ".jpeg" or ".png" or ".webp" or ".gif";
    }

    private static string GetContentType(string filePath)
    {
        var provider = new FileExtensionContentTypeProvider();
        return provider.TryGetContentType(filePath, out var contentType)
            ? contentType
            : "application/octet-stream";
    }
}