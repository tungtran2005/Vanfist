using System.ComponentModel.DataAnnotations;

namespace Vanfist.Entities;

public class Resource
{
    public int Id { get; set; }
    
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(255)]
    public string? Description { get; set; }
    
    // Navigation properties
    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}