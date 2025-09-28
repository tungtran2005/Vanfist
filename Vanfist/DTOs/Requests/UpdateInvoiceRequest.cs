using System.ComponentModel.DataAnnotations;

namespace Vanfist.DTOs.Requests
{
    public class UpdateInvoiceRequest
    {
        [Required]
        public int invoiceId { get; set; }

        [Required, StringLength(255)]
        public string Status { get; set; }

        [Required, StringLength(255)]
        public string Type { get; set; }
    }
}
