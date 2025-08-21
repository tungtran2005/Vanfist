using System.ComponentModel.DataAnnotations;

namespace Vanfist.DTOs.Requests;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(255)]
    public string Password { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"\nLoginRequest {{" +
               $"\n\tEmail: {Email}, " +
               $"\n\tPassword: {Password}" +
               $"\n}}";
    }
}