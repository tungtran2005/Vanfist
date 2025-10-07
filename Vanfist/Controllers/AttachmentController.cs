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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int modelId)
    {
        await _service.DeleteAsync(id);
        TempData["SuccessMessage"] = "Xóa file thành công!";
        return RedirectToAction("Update", "Model", new { id = modelId });
    }
}
