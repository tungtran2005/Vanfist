using System.ComponentModel.DataAnnotations;

namespace Vanfist.Models;

public class Account
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(255)]
    public string PasswordHash { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string? FirstName { get; set; }
    
    [StringLength(50)]
    public string? LastName { get; set; }

    [StringLength(20)] 
    public string? Number { get; set; }

    public ICollection<Role> Roles { get; set; } = new List<Role>();
}
