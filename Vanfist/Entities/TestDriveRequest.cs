using System.ComponentModel.DataAnnotations;

namespace Vanfist.Entities;

public class TestDriveRequest
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    public string FullName { get; set; } = null!;

    [EmailAddress, StringLength(120)]
    public string? Email { get; set; }

    [Phone, StringLength(20)]
    public string? Phone { get; set; }

    public int? ModelId { get; set; }
    public Model? Model { get; set; }

    public DateTime? PreferredTime { get; set; }

    [Required, StringLength(30)]
    public string Status { get; set; } = "New";   // New/Scheduled/Confirmed/Completed/Canceled

    [StringLength(1000)]
    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
