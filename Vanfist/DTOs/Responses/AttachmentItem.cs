namespace Vanfist.DTOs.Responses;

public class AttachmentItem
{
    public int Id { get; set; }
    public string FileName { get; set; } = "";

    // Đường dẫn hiển thị ảnh (VD: /uploads/123/xxx.png)
    public string Url { get; set; } = "";

    public string? ContentType { get; set; }
}
