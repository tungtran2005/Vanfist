using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vanfist.Constants;
using Vanfist.DTOs.Requests;
using Vanfist.DTOs.Responses;
using Vanfist.Services;

namespace Vanfist.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceApiController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IAccountService _accountService;

        public InvoiceApiController(IInvoiceService invoiceService, IAccountService accountService)
        {
            _invoiceService = invoiceService;
            _accountService = accountService;
        }

        // GET: api/invoiceapi/paged
        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<InvoiceResponse>>> GetPaged([FromQuery] InvoiceFilterRequest request)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? Role.User;

            var account = await _accountService.GetCurrentAccount();
            if (account == null)
            {
                return Unauthorized(new { message = "B?n ch?a ??ng nh?p" });
            }
            request.AccountId = account.Id;
            var result = await _invoiceService.GetPagedInvoice(request, role);
            return Ok(result);
        }
        // GET: api/InvoiceApi
        [HttpGet]

        public async Task<ActionResult<IEnumerable<InvoiceResponse>>> GetAll()
        {
            var invoices = await _invoiceService.GetAllInvoice();
            return Ok(invoices);
        }

        // GET: api/InvoiceApi/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceResponse>> GetById(int id)
        {
            var invoice = await _invoiceService.GetInvoice(id);
            if (invoice == null)
                return NotFound();
            return Ok(invoice);
        }

        // GET: api/InvoiceApi/account/{accountId}
        [HttpGet("account/{accountId}")]
        [Authorize(Roles = Role.User)]
        public async Task<ActionResult<IEnumerable<InvoiceResponse>>> GetByAccount()
        {
            var account = await _accountService.GetCurrentAccount();
            if (account == null)
            {
                return Unauthorized(new { message = "b?n ch?a ??ng nh?p" });
            }
            var invoices = await _invoiceService.GetAllInvoiceByAccountId(account.Id);
            return Ok(invoices);
        }

        // POST: api/InvoiceApi
        [HttpPost]
        public async Task<ActionResult<InvoiceResponse>> Create([FromBody] CreateInvoiceRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var invoice = await _invoiceService.CreateInvoice(request);
            return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, invoice);
        }

        // PUT: api/InvoiceApi
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateInvoiceRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _invoiceService.UpdateInvoice(request);
            if (!success)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/InvoiceApi/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _invoiceService.DeleteInvoice(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}