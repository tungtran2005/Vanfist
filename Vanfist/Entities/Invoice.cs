using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace Vanfist.Entities;

public class Invoice
{
    public int Id { get; set; }

    public int AccountId { get; set; }
    public Account Account { get; set; }
    
    public string Lastname { get; set; }
    public string FirstName { get; set; }
    public string Number { get; set; }
    public string Email { get; set; }

    [Required] public DateTime RequestDate { get; set; }

    [StringLength(255)] public string? Description { get; set; }

    [Required] public int ModelId { get; set; }
    public Model Model { get; set; }

    [Required] public float TotalPrice { get; set; }

    [Required] public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Required, StringLength(255)] public string Status { get; set; }

    [Required, StringLength(255)] public string Type { get; set; }
    
    [Required] public string City { get; set; }
    [Required] public string Details { get; set; }
}