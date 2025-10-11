using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Vanfist.Constants;
using Vanfist.DTOs.Requests;
using Vanfist.Models;
using Vanfist.Services;
using X.PagedList;
using X.PagedList.Extensions;

namespace Vanfist.Controllers;

public class HomeController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IModelService _modelService;
    private readonly IInvoiceService _invoiceService;

    public HomeController(
        IAccountService accountService, 
        IModelService modelService,
        IInvoiceService invoiceService)
    {
        _accountService = accountService;
        _modelService = modelService;
        _invoiceService = invoiceService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Index(PagingRequest request)
    {
        var models = await _modelService.FindAllModel();
        var pagedModels = models.ToPagedList(request.Page, request.PageSize);
        return View(pagedModels);
    }


    [AllowAnonymous]
    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var model = await _modelService.FindByIdModel(id);
        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Filter(FilterModelRequest request)
    {
        var models = await _modelService.FilterModel(request);
        return View("Index", models); // d�ng l?i view Index
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult SubmitConsultation(ConsultationRequest request)
    {
        try
        {
            _invoiceService.SubmitConsultation(request);
            TempData["SuccessMessage"] = "Đăng ký thành công";
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Đăng ký không thành công";
            Console.WriteLine(e);
            return RedirectToAction("Index");
        }
    }
}