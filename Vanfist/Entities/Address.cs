using System.ComponentModel.DataAnnotations;

namespace Vanfist.Entities;

public class Address
{
    public int Id { get; set; }

    [StringLength(255)] public string? City { get; set; }

    [StringLength(255)] public string? Detail { get; set; }

    public int AccountId { get; set; }
    public Account Account { get; set; }
}