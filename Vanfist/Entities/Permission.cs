using System.ComponentModel.DataAnnotations;

namespace Vanfist.Models;

public class Permission
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(200)]
    public string? Description { get; set; } = string.Empty;
    
    // Foreign keys
    public int ActionId { get; set; }
    public int ResourceId { get; set; }
    
    // Navigation properties
    public ICollection<Role> Roles { get; set; } = new List<Role>();
    public ActionEntity Action { get; set; } = null!;
    public Resource Resource { get; set; } = null!;
}