using System.ComponentModel.DataAnnotations;

namespace Vanfist.DTOs.Requests;

public class UpdateModelRequest
{
    [Required]
    public int Id { get; set; }

    [Required, StringLength(255)]
    public string Name { get; set; }

    [Required] public float Price { get; set; }
    [Required] public float Length { get; set; }
    [Required] public float Width { get; set; }
    [Required] public float Height { get; set; }
    [Required] public float Wheelbase { get; set; }
    [Required] public float NEDC { get; set; }
    [Required] public float MaximumPower { get; set; }
    [Required] public float MaximumTorque { get; set; }
    [Required] public float RimSize { get; set; }

    [Required, StringLength(100)]
    public string Color { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public List<IFormFile>? Attachments { get; set; }
}
