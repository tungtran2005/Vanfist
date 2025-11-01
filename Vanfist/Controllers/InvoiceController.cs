using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vanfist.Constants;
using Vanfist.DTOs.Requests;
using Vanfist.Services;
using Vanfist.Services.Impl;

namespace Vanfist.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IAccountService _accountService;

        public InvoiceController(IInvoiceService invoiceService, IAccountService accountService)
        {
            _invoiceService = invoiceService;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPagedInvoicePartial(InvoiceFilterRequest request)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? Role.User;

            var account = await _accountService.GetCurrentAccount();
            if (account == null)
            {
                return Unauthorized(new { message = "Bạn chưa đăng nhập" });
            }
            request.AccountId = account.Id;
            var result = await _invoiceService.GetPagedInvoice(request, role);

            return PartialView("_InvoiceListPartial", result);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var success = await _invoiceService.DeleteInvoice(id);
            if (!success)
                return Json(new { success = false, message = "Không tìm thấy hóa đơn hoặc đã bị xóa." });

            return Json(new { success = true, message = "Xóa hóa đơn thành công!" });
        }
        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> UpdateInvoice([FromBody] UpdateInvoiceRequest request)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Dữ liệu không hợp lệ." });

            var result = await _invoiceService.UpdateInvoice(request);
            if (result)
                return Json(new { success = true, message = "Cập nhật trạng thái hóa đơn thành công!" });

            return Json(new { success = false, message = "Không tìm thấy hóa đơn cần cập nhật." });
        }
        [HttpPost]
        [Authorize(Roles = Role.UserAndAdmin)]
        public async Task<IActionResult> CreateInvoice([FromForm] CreateInvoiceRequest request)
        {
            var account = await _accountService.GetCurrentAccount();
            if (account == null)
                return Json(new { success = false, message = "Bạn chưa đăng nhập." });

            request.AccountId = account.Id;
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
            try
            {
                var invoice = await _invoiceService.CreateInvoice(request);

                return Json(new
                {
                    success = true,
                    message = "Tạo hóa đơn thành công!",
                    id = invoice.Id,
                    data = invoice
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi tạo hóa đơn.", error = ex.Message });
            }
        }
    }
}
