using Microsoft.Build.Framework;

namespace Vanfist.DTOs.Requests
{
    public class ConsultationRequest
    {
        [Required] public string LastName { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string Number { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Description { get; set; }
        [Required] public int ModelId { get; set; }
        [Required] public string City { get; set; }
        [Required] public string Details { get; set; }
    }
}