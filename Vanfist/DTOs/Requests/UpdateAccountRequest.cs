using System.ComponentModel.DataAnnotations;

namespace Vanfist.DTOs.Requests;

public class UpdateAccountRequest
{
    
    [StringLength(255, ErrorMessage = "First name must not exceed 255 characters.")]
    public string FirstName { get; set; } = string.Empty;
    
    [StringLength(255, ErrorMessage = "Last name must not exceed 255 characters.")]
    public string LastName { get; set; } = string.Empty;
}