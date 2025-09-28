using Vanfist.DTOs.Requests;
using Vanfist.DTOs.Responses;
using Vanfist.Entities;
using Vanfist.Repositories;
using Vanfist.Services.Base;

namespace Vanfist.Services.Impl
{
    public class InvoiceService : Service, IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<InvoiceResponse> CreateInvoice(InvoiceRequest request)
        {
            var invoice = new Invoice
            {
                AccountId = request.AccountId,
                ModelId = request.ModelId,
                RequestDate = request.RequestDate,
                Description = request.Description,
                TotalPrice = request.TotalPrice,
                Status = request.Status,
                Type = request.Type,
            };
            await _invoiceRepository.Save(invoice);
            await _invoiceRepository.SaveChanges();
            return InvoiceResponse.From(invoice);
        }
        public async Task<IEnumerable<InvoiceResponse>> GetAllInvoice()
        {
            var invoices = await _invoiceRepository.FindAll();
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
            var invoice = await _invoiceRepository.FindById(request.invoiceId);
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
    }
}
