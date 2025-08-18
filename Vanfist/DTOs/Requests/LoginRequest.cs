using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;

namespace Vanfist.DTOs.Requests;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(255)]
    public string Password { get; set; } = string.Empty;
}