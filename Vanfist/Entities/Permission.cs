using System.ComponentModel.DataAnnotations;

namespace Vanfist.Entities;

public class Permission
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(255)]
    public string? Description { get; set; } = string.Empty;
    
    // Foreign keys
    public int ActionId { get; set; }
    public int ResourceId { get; set; }
    
    // Navigation properties
    public ICollection<Role> Roles { get; set; } = new List<Role>();
    public ActionEntity Action { get; set; } = null!;
    public Resource Resource { get; set; } = null!;
}