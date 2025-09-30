using Vanfist.Entities;

namespace Vanfist.DTOs.Responses;

public class ModelResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float Price { get; set; }
    public float Length { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public float Wheelbase { get; set; }
    public float NEDC { get; set; }
    public float MaximumPower { get; set; }
    public float MaximumTorque { get; set; }
    public float RimSize { get; set; }
    public string Color { get; set; }

    public int CategoryId { get; set; }
    public string CategoryName { get; set; }

    public List<string> AttachmentUrls { get; set; } = new List<string>();

    public static ModelResponse FromEntity(Model model)
    {
        return new ModelResponse
        {
            Id = model.Id,
            Name = model.Name,
            Price = model.Price,
            Length = model.Length,
            Width = model.Width,
            Height = model.Height,
            Wheelbase = model.Wheelbase,
            NEDC = model.NEDC,
            MaximumPower = model.MaximumPower,
            MaximumTorque = model.MaximumTorque,
            RimSize = model.RimSize,
            Color = model.Color,
            CategoryId = model.CategoryId,
            CategoryName = model.Category?.Name ?? string.Empty,
            AttachmentUrls = model.Attachments?
            .Select(a => "/uploads/" + a.FileName) // => /uploads/c85daeb1.jpg
            .ToList() ?? new List<string>()
        };
    }
}
