using System.ComponentModel.DataAnnotations;

namespace Vanfist.DTOs.Requests;

public class UpdateAccountRequest
{
    
    [StringLength(255, ErrorMessage = "Tên có nhiều nhất 255 chữ cái.")]
    public string FirstName { get; set; } = string.Empty;
    
    [StringLength(255, ErrorMessage = "Họ có nhiều nhất 255 chữ cái.")]
    public string LastName { get; set; } = string.Empty;
    
    [StringLength(255, ErrorMessage = "Địa chỉ chi tiết có nhiều nhất 255 chữ cái.")]
    public string Detail { get; set; } = string.Empty;
    
    [StringLength(255, ErrorMessage = "Thành phố có nhiều nhất 255 chữ cái.")]
    public string City { get; set; } = string.Empty;
}