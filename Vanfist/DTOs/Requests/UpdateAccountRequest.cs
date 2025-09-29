using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Vanfist.DTOs.Requests;

public class UpdateAccountRequest
{
    [Required(ErrorMessage = "Tên không được để trống.")]
    [StringLength(255, ErrorMessage = "Tên có nhiều nhất 255 chữ cái.")]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "Họ không được để trống.")]
    [StringLength(255, ErrorMessage = "Họ có nhiều nhất 255 chữ cái.")]
    public string LastName { get; set; }
    
    [Required(ErrorMessage = "Số điện thoại không được để trống.")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải gồm 10 chữ số.")]
    public string? Number { get; set; }
    
    [Required(ErrorMessage = "Địa chỉ chi tiết không được để trống.")]
    [StringLength(255, ErrorMessage = "Địa chỉ chi tiết có nhiều nhất 255 chữ cái.")]
    public string Detail { get; set; }
    
    [Required(ErrorMessage = "Thành phố không được để trống.")]
    [StringLength(255, ErrorMessage = "Thành phố có nhiều nhất 255 chữ cái.")]
    public string City { get; set; }
}