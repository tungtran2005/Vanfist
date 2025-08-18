using System.ComponentModel.DataAnnotations;
using Vanfist.Entities;

namespace Vanfist.Entities;

public class Role
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(255)]
    public string? Description { get; set; }
    
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
    
    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
