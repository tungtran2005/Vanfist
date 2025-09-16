using System.ComponentModel.DataAnnotations;

namespace Vanfist.DTOs.Requests;

public class ChangePasswordRequest
{
    [Required(ErrorMessage = "Old password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    [MaxLength(50, ErrorMessage = "Password must not exceed 50 characters.")]
    public string OldPassword { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "New password is required.")]
    [MinLength(8, ErrorMessage = "New password must be at least 8 characters.")]
    [MaxLength(50, ErrorMessage = "New password must not exceed 50 characters.")]
    public string NewPassword { get; set; } = string.Empty;
}