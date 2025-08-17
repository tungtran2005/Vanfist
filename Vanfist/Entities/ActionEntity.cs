using System.ComponentModel.DataAnnotations;

namespace Vanfist.Models;

public class ActionEntity
{
    public int Id { get; set; }
    
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(200)]
    public string? Description { get; set; }
    
    // Navigation properties
    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}