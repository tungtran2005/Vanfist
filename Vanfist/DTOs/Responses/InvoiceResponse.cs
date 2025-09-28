using Vanfist.Entities;

namespace Vanfist.DTOs.Responses
{
    public class InvoiceResponse
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int ModelId { get; set; }
        public float TotalPrice { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }

        public InvoiceResponse(int id, int accountId, int modelId, float totalPrice, string status, string type, DateTime createdAt)
        {
            Id = id;
            AccountId = accountId;
            ModelId = modelId;
            TotalPrice = totalPrice;
            Status = status;
            Type = type;
            CreatedAt = createdAt;
        }

        public static InvoiceResponse From(Invoice invoice)
        {
            return new InvoiceResponse(
                invoice.Id,
                invoice.AccountId,
                invoice.ModelId,
                invoice.TotalPrice,
                invoice.Status,
                invoice.Type,
                invoice.CreatedAt
            );
        }

        public override string ToString()
        {
            return $"\nInvoiceResponse {{" +
                   $"\n\tId: {Id}," +
                   $"\n\tAccountId: {AccountId}," +
                   $"\n\tModelId: {ModelId}," +
                   $"\n\tTotalPrice: {TotalPrice}," +
                   $"\n\tStatus: {Status}," +
                   $"\n\tType: {Type}," +
                   $"\n\tCreatedAt: {CreatedAt}" +
                   $"\n}}";
        }
    }
}