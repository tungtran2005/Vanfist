using System.ComponentModel.DataAnnotations;

namespace Vanfist.DTOs.Requests
{
    public class CreateInvoiceRequest
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

        [Required, StringLength(255)]
        public string Lastname { get; set; }

        [Required, StringLength(255)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string Number { get; set; }

        [Required, StringLength(255)]
        public string Email { get; set; }

        [Required, StringLength(255)]
        public string City { get; set; }

        [Required, StringLength(255)]
        public string Details { get; set; }
    }
}
