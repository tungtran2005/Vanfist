using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Vanfist.DTOs.Requests;
using Vanfist.Services;

namespace Vanfist.Controllers;

public class TestDriveController : Controller
{
    private readonly ITestDriveService _service;
    private readonly IModelService _modelService;   // thêm service model
    private readonly ILogger<TestDriveController> _logger;

    public TestDriveController(
        ITestDriveService service,
        IModelService modelService,
        ILogger<TestDriveController> logger)
    {
        _service = service;
        _modelService = modelService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var models = await _modelService.FindAllModel();
        ViewBag.Models = models; // gửi qua View
        return View(new TestDriveRequestCreate());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(TestDriveRequestCreate model)
    {
        if (!ModelState.IsValid)
        {
            var models = await _modelService.FindAllModel();
            ViewBag.Models = models;
            return View(model);
        }

        try
        {
            var id = await _service.Create(model);
            TempData["Success"] = "Đăng ký thành công! Chúng tôi sẽ liên hệ sớm.";
            return RedirectToAction(nameof(Success), new { id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Create TestDriveRequest failed");

            var models = await _modelService.FindAllModel();
            ViewBag.Models = models;

            ModelState.AddModelError("", "Có lỗi xảy ra. Vui lòng thử lại.");
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Success(int id)
    {
        var item = await _service.Get(id);
        if (item == null) return RedirectToAction(nameof(Index));
        return View(item);
    }
}
