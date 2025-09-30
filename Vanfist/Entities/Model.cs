using System.ComponentModel.DataAnnotations;

namespace Vanfist.Entities;

public class Model
{
    public int Id { get; set; }

    [Required, StringLength(255)] public string Name { get; set; }

    [Required] public float Price { get; set; }
    [Required] public float Length { get; set; } // mm
    [Required] public float Width { get; set; } // mm
    [Required] public float Height { get; set; } // mm
    [Required] public float Wheelbase { get; set; } // Chiều dài cơ sở - VD : 2730 mm
    [Required] public float NEDC { get; set; } // Quãng đường di chuển (NEDC) - VD : 500 km/lần sạc
    [Required] public float MaximumPower { get; set; } // Công suất tối đa - VD : 150 kW/174 hp
    [Required] public float MaximumTorque { get; set; } // Mô men xoắn tối đa - VD : 310 Nm
    [Required] public float RimSize { get; set; } // Kích thước La-zăng - VD : 18 inch
    [Required] public string Color { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}