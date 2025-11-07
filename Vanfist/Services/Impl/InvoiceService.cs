using Microsoft.EntityFrameworkCore;
using Vanfist.Constants;
using Vanfist.DTOs.Requests;
using Vanfist.DTOs.Responses;
using Vanfist.Entities;
using Vanfist.Repositories;
using Vanfist.Services.Base;
using Invoice = Vanfist.Entities.Invoice;

namespace Vanfist.Services.Impl
{
    public class InvoiceService : Service, IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IAccountService _accountService;
        private readonly IModelService _modelService;
        public InvoiceService(
            IInvoiceRepository invoiceRepository,
            IAccountService accountService,
            IModelService modelService)
        {
            _invoiceRepository = invoiceRepository;
            _accountService = accountService;
            _modelService = modelService;
        }


        public async Task<InvoiceResponse> CreateInvoice(CreateInvoiceRequest request)
        {
            var account = await _accountService.GetCurrentAccount();
            var invoice = new Entities.Invoice
            {
                AccountId = account.Id,
                ModelId = request.ModelId,
                RequestDate = request.RequestDate,
                Description = request.Description,
                TotalPrice = request.TotalPrice,
                Type = Vanfist.Constants.Invoice.Type.Service,
                Status = Vanfist.Constants.Invoice.Status.Pending,
                City = request.City,
                Details = request.Details,
                Email = request.Email,
                Lastname = request.Lastname,
                FirstName = request.FirstName,
                Number = request.Number
            };
            await _invoiceRepository.Save(invoice);
            await _invoiceRepository.SaveChanges();
            var model = await _modelService.FindByIdModel(request.ModelId);
            invoice.Model.Name = model.Name;
            return InvoiceResponse.From(invoice);
        }

        public async Task<IEnumerable<InvoiceResponse>> GetAllInvoice()
        {
            var invoices = await _invoiceRepository.FindAll();
            return invoices.Select(InvoiceResponse.From).ToList();
        }

        public async Task<IEnumerable<InvoiceResponse>> GetAllInvoiceByAccountId(int accountId)
        {
            var invoices = await _invoiceRepository.FindByAccountId(accountId);
            return invoices.Select(InvoiceResponse.From).ToList();
        }

        public async Task<InvoiceResponse?> GetInvoice(int invoiceId)
        {
            var invoice = await _invoiceRepository.FindById(invoiceId);
            if (invoice == null)
            {
                return null;
            }

            return InvoiceResponse.From(invoice);
        }

        public async Task<bool> UpdateInvoice(UpdateInvoiceRequest request)
        {
            var invoice = await _invoiceRepository.FindById(request.InvoiceId);
            if (invoice == null)
            {
                return false;
            }

            invoice.Status = request.Status;
            invoice.Type = request.Type;
            await _invoiceRepository.Update(invoice);
            await _invoiceRepository.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteInvoice(int invoiceId)
        {
            var invoice = await _invoiceRepository.FindById(invoiceId);
            if (invoice == null)
            {
                return false;
            }

            await _invoiceRepository.Delete(invoice);
            await _invoiceRepository.SaveChanges();
            return true;
        }

        public async Task<PagedResult<InvoiceResponse>> GetPagedInvoice(InvoiceFilterRequest request, string role)
        {
            var query = _invoiceRepository.Query();

            if (role != Constants.Role.Admin)
            {
                query = query.Where(i => i.AccountId == request.AccountId);
            }

            if (!string.IsNullOrEmpty(request.Status))
                query = query.Where(i => i.Status == request.Status);

            if (!string.IsNullOrEmpty(request.Type))
                query = query.Where(i => i.Type == request.Type);

            var totalCount = await query.CountAsync();

            var invoices = await query
                .OrderByDescending(i => i.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var responses = invoices.Select(InvoiceResponse.From).ToList();

            return new PagedResult<InvoiceResponse>
            {
                Page = request.Page,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                Items = responses
            };
        }

        public void SubmitConsultation(ConsultationRequest request)
        {
            var account = _accountService.GetCurrentAccount();

            if (account != null)
            {

                var invoice1 = new Invoice()
                {
                    AccountId = account.Id,
                    RequestDate = DateTime.Now,
                    Description = request.Description,
                    ModelId = request.ModelId,
                    TotalPrice = 0,
                    Type = Vanfist.Constants.Invoice.Type.Service,
                    Status = Vanfist.Constants.Invoice.Status.Pending,
                    City = request.City,
                    Details = request.Details,
                    Email = request.Email,
                    Lastname = request.LastName,
                    FirstName = request.FirstName,
                    Number = request.Number


                };

                _invoiceRepository.Save(invoice1);
                _invoiceRepository.SaveChanges();
                return;
            }

            var invoice2 = new Invoice()
            {
                Lastname = request.LastName,
                FirstName = request.FirstName,
                Number = request.Number,
                Email = request.Email,
                RequestDate = DateTime.Now,
                Description = request.Description,
                ModelId = request.ModelId,
                TotalPrice = 0,
                Type = Vanfist.Constants.Invoice.Type.Service,
                Status = Vanfist.Constants.Invoice.Status.Pending,
                City = request.City,
                Details = request.Details
            };

            _invoiceRepository.Save(invoice2);
            _invoiceRepository.SaveChanges();
        }
    }
}