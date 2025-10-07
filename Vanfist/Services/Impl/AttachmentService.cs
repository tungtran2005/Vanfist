using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Vanfist.Configuration.Database;
using Vanfist.Entities;

namespace Vanfist.Services.Impl;

public class AttachmentService : IAttachmentService
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public AttachmentService(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    private string GetModelDir(int modelId)
        => Path.Combine(_env.WebRootPath, "uploads", modelId.ToString());

    private static string GetContentType(string path)
    {
        var provider = new FileExtensionContentTypeProvider();
        return provider.TryGetContentType(path, out var ct) ? ct : "application/octet-stream";
    }

    public async Task<Attachment> UploadAsync(IFormFile file, int modelId)
    {
        Directory.CreateDirectory(GetModelDir(modelId));

        var safeName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var fullPath = Path.Combine(GetModelDir(modelId), safeName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var attachment = new Attachment
        {
            FileName = safeName,                   // LƯU tên đã đổi (GUID.ext) để tránh trùng
            Type = file.ContentType ?? GetContentType(fullPath),
            ModelId = modelId
        };

        _context.Attachments.Add(attachment);
        await _context.SaveChangesAsync();

        return attachment;
    }

    public async Task<IEnumerable<Attachment>> ListByModel(int modelId)
    {
        return await _context.Attachments
            .Where(a => a.ModelId == modelId)
            .OrderByDescending(a => a.Id)
            .ToListAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var att = await _context.Attachments.FindAsync(id);
        if (att == null) return;

        var fullPath = Path.Combine(GetModelDir(att.ModelId), att.FileName);
        if (File.Exists(fullPath)) File.Delete(fullPath);

        _context.Attachments.Remove(att);
        await _context.SaveChangesAsync();
    }
}
