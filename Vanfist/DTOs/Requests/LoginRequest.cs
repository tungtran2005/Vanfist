using System.ComponentModel.DataAnnotations;

namespace Vanfist.DTOs.Requests;

public class LoginRequest
{
    [Required(ErrorMessage = "Email không được bỏ trống.")] 
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")] 
    [StringLength(255)] 
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Mật khẩu không đươc để trống.")] 
    [StringLength(255)] 
    public string Password { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"\nLoginRequest {{" +
               $"\n\tEmail: {Email}, " +
               $"\n\tPassword: {Password}" +
               $"\n}}";
    }
}