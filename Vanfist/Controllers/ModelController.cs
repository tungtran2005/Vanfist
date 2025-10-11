using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vanfist.DTOs.Requests;
using Vanfist.Services;
using Vanfist.Services.Impl;

namespace Vanfist.Controllers;

[Authorize(Roles = Constants.Role.Admin)] // chỉ Admin mới CRUD
public class ModelController : Controller
{
    private readonly IModelService _modelService;
    private readonly ICategoryService _categoryService;
    private readonly IAttachmentService _attachmentService;

    public ModelController(IModelService modelService, ICategoryService categoryService, IAttachmentService attachmentService)
    {
        _modelService = modelService;
        _categoryService = categoryService;
        _attachmentService = attachmentService;
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
    public async Task<IActionResult> Create(AddModelRequest request)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _categoryService.FindAllCategories();
            ViewBag.Categories = categories;
            return View(request);
        }

        await _modelService.AddModel(request);
        TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var model = await _modelService.GetUpdateModelRequest(id);
        if (model == null) return NotFound();

        ViewBag.Categories = await _categoryService.FindAllCategories();
        ViewBag.Attachments = await _attachmentService.ListByModel(id);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(UpdateModelRequest request)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _categoryService.FindAllCategories();
            ViewBag.Attachments = await _attachmentService.ListByModel(request.Id);
            return View(request);
        }

        try
        {
            var updatedModel = await _modelService.UpdateModel(request);

            if (updatedModel == null) // service trả về null nếu không tìm thấy
            {
                ModelState.AddModelError("", "Không tìm thấy sản phẩm để cập nhật!");
                ViewBag.Categories = await _categoryService.FindAllCategories();
                ViewBag.Attachments = await _attachmentService.ListByModel(request.Id);
                return View(request);
            }

            TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            // log lỗi nếu cần
            ModelState.AddModelError("", $"Có lỗi xảy ra: {ex.Message}");
            ViewBag.Categories = await _categoryService.FindAllCategories();
            ViewBag.Attachments = await _attachmentService.ListByModel(request.Id);
            return View(request);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var model = await _modelService.FindByIdModel(id);
        if (model == null) return NotFound();

        return View(model); // model kiểu ModelResponse
    }


    [HttpPost]
    public async Task<IActionResult> Delete(DeleteModelRequest request)
    {
        try
        {
            await _modelService.DeleteModel(request);
            TempData["SuccessMessage"] = "Xóa sản phẩm thành công!";
            return RedirectToAction("Index", "Home");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }


}
