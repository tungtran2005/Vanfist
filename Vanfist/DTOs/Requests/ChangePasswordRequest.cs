using System.ComponentModel.DataAnnotations;

namespace Vanfist.DTOs.Requests;

public class ChangePasswordRequest
{
    [Required(ErrorMessage = "Mật khẩu cũ không được bỏ trống.")]
    public string OldPassword { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Mật khẩu mới không được bỏ trống.")]
    [MinLength(8, ErrorMessage = "Mật kẩu mới phải có ít nhất 8 ký tự.")]
    [MaxLength(50, ErrorMessage = "Mật khẩu mới có nhiều nhất 50 ký tự.")]
    public string NewPassword { get; set; } = string.Empty;
}