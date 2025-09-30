using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vanfist.DTOs.Requests;
using Vanfist.Services;

namespace Vanfist.Controllers;

[Authorize(Roles = "Admin")] // chỉ Admin mới CRUD
public class ModelController : Controller
{
    private readonly IModelService _modelService;
    private readonly ICategoryService _categoryService;

    public ModelController(IModelService modelService, ICategoryService categoryService)
    {
        _modelService = modelService;
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var categories = await _categoryService.FindAllCategories();
        ViewBag.Categories = categories;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AddModelRequest request, IFormFile? imageFile)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _categoryService.FindAllCategories();
            ViewBag.Categories = categories;
            return View(request);
        }

        await _modelService.AddModel(request, imageFile);
        TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _modelService.FindByIdModel(id);
        if (model == null)
        {
            return NotFound();
        }

        var categories = await _categoryService.FindAllCategories();
        ViewBag.Categories = categories;

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateModelRequest request, IFormFile? imageFile)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _categoryService.FindAllCategories();
            ViewBag.Categories = categories;
            return View(request);
        }

        await _modelService.UpdateModel(request, imageFile);
        TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(DeleteModelRequest request)
    {
        await _modelService.DeleteModel(request);
        TempData["SuccessMessage"] = "Xóa sản phẩm thành công!";
        return RedirectToAction("Index", "Home");
    }
}
