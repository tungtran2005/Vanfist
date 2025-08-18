using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;

namespace Vanfist.DTOs.Requests;

public class RegisterRequest
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [StringLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    [MaxLength(50, ErrorMessage = "Password must not exceed 50 characters.")]
    public string Password { get; set; } = string.Empty;
    
    [StringLength(255, ErrorMessage = "First name must not exceed 255 characters.")]
    public string FirstName { get; set; } = string.Empty;
    
    [StringLength(255, ErrorMessage = "Last name must not exceed 255 characters.")]
    public string LastName { get; set; } = string.Empty;
    
    [StringLength(255, ErrorMessage = "Phone number must not exceed 255 characters.")]
    public string Number { get; set; } = string.Empty;
}