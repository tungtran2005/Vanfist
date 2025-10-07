using System.ComponentModel.DataAnnotations;

namespace Vanfist.Entities;

public class Attachment
{
    public int Id { get; set; }

    [Required, StringLength(255)]
    public string FileName { get; set; } = null!;

    [Required, StringLength(255)]
    public string Type { get; set; } = null!;   // image/png, application/pdf ...
    public int ModelId { get; set; }
    public Model Model { get; set; } = null!;
}
