using Vanfist.DTOs.Requests;
using Vanfist.DTOs.Responses;
using Vanfist.Services.Base;

namespace Vanfist.Services
{
    public interface IInvoiceService : IService
    {
        Task<InvoiceResponse> CreateInvoice(InvoiceRequest request);
        Task<IEnumerable<InvoiceResponse>> GetAllInvoice();
        Task<InvoiceResponse?> GetInvoice(int invoiceId);
        Task<bool> UpdateInvoice(UpdateInvoiceRequest request);
        Task<bool> DeleteInvoice(int invoiceId);
    }
}
