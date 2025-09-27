using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace Vanfist.Entities;

public class Invoice
{
    public int Id { get; set; }

    [Required] public int AccountId { get; set; }
    public Account Account { get; set; }

    [Required] public DateTime RequestDate { get; set; }

    [StringLength(255)] public string? Description { get; set; }

    [Required] public int ModelId { get; set; }
    public Model Model { get; set; }

    [Required] public float TotalPrice { get; set; }

    [Required] public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Required, StringLength(255)] public string Status { get; set; }

    [Required, StringLength(255)] public string Type { get; set; }
}