using System.ComponentModel.DataAnnotations;

namespace Vanfist.DTOs.Requests
{
    public class InvoiceRequest
    {
        [Required(ErrorMessage = "AccountId is required.")]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "ModelId is required.")]
        public int ModelId { get; set; }

        [Required(ErrorMessage = "RequestDate is required.")]
        public DateTime RequestDate { get; set; }

        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "TotalPrice is required.")]
        [Range(0, float.MaxValue, ErrorMessage = "TotalPrice must be a positive number.")]
        public float TotalPrice { get; set; }

        [Required, StringLength(255)]
        public string Status { get; set; }

        [Required, StringLength(255)]
        public string Type { get; set; }
    }
}
