using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;

namespace Vanfist.DTOs.Requests;

public class RegisterRequest
{
    [Required(ErrorMessage = "Email không được bỏ trống.")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    [StringLength(100, ErrorMessage = "Email có nhiều nhất 100 ký tự.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mật khẩu không được bỏ trống.")]
    [MinLength(8, ErrorMessage = "Mật khẩu có ít nhất 8 ký tự.")]
    [MaxLength(50, ErrorMessage = "Mật khẩu có nhiều nhất 50 ký tự.")]
    public string Password { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "Tên có nhiều nhất 255 ký tự.")]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "Họ có nhiều nhất 255 ký tự.")]
    public string LastName { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "Số điện thoại không hợp lệ.")]
    public string Number { get; set; } = string.Empty;
}