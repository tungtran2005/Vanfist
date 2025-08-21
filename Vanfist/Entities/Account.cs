using System.ComponentModel.DataAnnotations;

namespace Vanfist.Entities;

public class Account
{
    public int Id { get; set; }

    [Required, StringLength(255)]
    [EmailAddress]
    public string Email { get; set; }

    [Required, StringLength(255)] public string Password { get; set; }

    [StringLength(255)] public string? FirstName { get; set; }

    [StringLength(255)] public string? LastName { get; set; }

    [StringLength(20)] public string? Number { get; set; }

    public ICollection<Role> Roles { get; set; } = new List<Role>();

    public ICollection<Address> Addresses { get; set; } = new List<Address>();

    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public override string ToString()
    {
        return $"\nAccount {{" +
               $"\nId: {Id}, " +
               $"\nEmail: {Email}, " +
               $"\nFirstName: {FirstName}, " +
               $"\nLastName: {LastName}, " +
               $"\nNumber: {Number}" +
               $"}}";
    }
}