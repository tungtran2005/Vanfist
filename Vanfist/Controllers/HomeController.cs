using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vanfist.Constants;
using Vanfist.Models;
using Vanfist.Services;
using X.PagedList;
using X.PagedList.Extensions;

namespace Vanfist.Controllers;

public class HomeController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IModelService _modelService;

    public HomeController(
        IAccountService accountService, IModelService modelService)
    {
        _accountService = accountService;
        _modelService = modelService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 8)
    {
        var models = await _modelService.FindAllModel(); // l?y toàn b? s?n ph?m
        var pagedModels = models.ToPagedList(page, pageSize);
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
}