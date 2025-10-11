namespace Vanfist.DTOs.Requests
{
    public class InvoiceFilterRequest
    {
        public int? AccountId { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
