using Microsoft.AspNetCore.Mvc;
using Vanfist.Services;

namespace Vanfist.Controllers;

public class AttachmentController : Controller
{
    private readonly IAttachmentService _service;

    public AttachmentController(IAttachmentService service)
    {
        _service = service;
    }

    // Upload one file (existing)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(int modelId, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            TempData["ErrorMessage"] = "Chưa chọn file.";
            return RedirectToAction("Update", "Model", new { id = modelId });
        }

        await _service.UploadAsync(file, modelId);
        TempData["SuccessMessage"] = "Upload thành công!";
        return RedirectToAction("Update", "Model", new { id = modelId });
    }

    // Upload many files
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadMany(int modelId, List<IFormFile>? files)
    {
        if (files == null || files.Count == 0 || files.All(f => f == null || f.Length == 0))
        {
            TempData["ErrorMessage"] = "Chưa chọn file.";
            return RedirectToAction("Update", "Model", new { id = modelId });
        }

        var uploaded = 0;
        foreach (var f in files.Where(f => f != null && f.Length > 0))
        {
            await _service.UploadAsync(f, modelId);
            uploaded++;
        }

        TempData["SuccessMessage"] = uploaded > 0
            ? $"Đã upload {uploaded} ảnh."
            : "Không có ảnh hợp lệ để upload.";

        return RedirectToAction("Update", "Model", new { id = modelId });
    }

    // Delete one file (existing)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int modelId)
    {
        await _service.DeleteAsync(id);
        TempData["SuccessMessage"] = "Xóa file thành công!";
        return RedirectToAction("Update", "Model", new { id = modelId });
    }

    // Delete many files
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteMany(int modelId, List<int>? ids)
    {
        if (ids == null || ids.Count == 0)
        {
            TempData["ErrorMessage"] = "Chưa chọn ảnh để xóa.";
            return RedirectToAction("Update", "Model", new { id = modelId });
        }

        var distinctIds = ids.Distinct().ToList();
        foreach (var id in distinctIds)
        {
            await _service.DeleteAsync(id);
        }

        TempData["SuccessMessage"] = $"Đã xóa {distinctIds.Count} ảnh.";
        return RedirectToAction("Update", "Model", new { id = modelId });
    }
}
