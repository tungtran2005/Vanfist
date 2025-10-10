using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Vanfist.Entities;

namespace Vanfist.DTOs.Requests;

public class UpdateModelRequest : IValidatableObject
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
    [StringLength(255, ErrorMessage = "Tên không được vượt quá 255 ký tự")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Giá là bắt buộc")]
    [Range(1, double.MaxValue, ErrorMessage = "Giá phải là số dương")]
    public float Price { get; set; }

    [Required(ErrorMessage = "Chiều dài là bắt buộc")]
    [Range(1, double.MaxValue, ErrorMessage = "Chiều dài phải là số dương")]
    public float Length { get; set; }

    [Required(ErrorMessage = "Chiều rộng là bắt buộc")]
    [Range(1, double.MaxValue, ErrorMessage = "Chiều rộng phải là số dương")]
    public float Width { get; set; }

    [Required(ErrorMessage = "Chiều cao là bắt buộc")]
    [Range(1, double.MaxValue, ErrorMessage = "Chiều cao phải là số dương")]
    public float Height { get; set; }

    [Required(ErrorMessage = "Chiều dài cơ sở là bắt buộc")]
    [Range(1, double.MaxValue, ErrorMessage = "Chiều dài cơ sở phải là số dương")]
    public float Wheelbase { get; set; }

    [Required(ErrorMessage = "NEDC là bắt buộc")]
    [Range(1, double.MaxValue, ErrorMessage = "NEDC phải là số dương")]
    public float NEDC { get; set; }

    [Required(ErrorMessage = "Công suất cực đại là bắt buộc")]
    [Range(1, double.MaxValue, ErrorMessage = "Công suất cực đại phải là số dương")]
    public float MaximumPower { get; set; }

    [Required(ErrorMessage = "Mô-men xoắn cực đại là bắt buộc")]
    [Range(1, double.MaxValue, ErrorMessage = "Mô-men xoắn cực đại phải là số dương")]
    public float MaximumTorque { get; set; }

    [Required(ErrorMessage = "Kích thước mâm xe là bắt buộc")]
    [Range(1, double.MaxValue, ErrorMessage = "Kích thước mâm xe phải là số dương")]
    public float RimSize { get; set; }

    [Required(ErrorMessage = "Màu sắc là bắt buộc")]
    [RegularExpression(@"^[a-zA-ZÀ-ỹ\s]+$", ErrorMessage = "Màu chỉ được chứa chữ cái")]
    [StringLength(100, ErrorMessage = "Màu không được vượt quá 100 ký tự")]
    public string Color { get; set; } = null!;

    [Required(ErrorMessage = "Danh mục là bắt buộc")]
    public int CategoryId { get; set; }

    // Ảnh mới upload (không bắt buộc, trừ khi xóa hết ảnh hiện có)
    public List<IFormFile>? Attachments { get; set; }

    // Ảnh cũ cần xóa (danh sách Id)
    public List<int>? DeletedAttachmentIds { get; set; }

    // Danh sách Id ảnh hiện có (được POST lại qua hidden inputs để validate)
    public List<int>? ExistingAttachmentIds { get; set; }

    // Không bind từ client; chỉ để hiển thị khi cần
    public List<Attachment>? ExistingAttachments { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Số ảnh còn lại sau khi xóa
        var existingIds = ExistingAttachmentIds ?? new List<int>();
        var deletedIds = DeletedAttachmentIds?.Distinct().ToHashSet() ?? new HashSet<int>();
        var remaining = existingIds.Count(id => !deletedIds.Contains(id));

        var hasNewUploads = Attachments != null && Attachments.Any(f => f != null && f.Length > 0);

        // Nếu xóa hết ảnh hiện có thì bắt buộc phải upload ít nhất 1 ảnh mới
        if (remaining <= 0 && !hasNewUploads)
        {
            yield return new ValidationResult(
                "Nếu xóa hết ảnh hiện có, bạn phải tải lên ít nhất một ảnh mới.",
                new[] { nameof(Attachments) }
            );
        }
    }
}
