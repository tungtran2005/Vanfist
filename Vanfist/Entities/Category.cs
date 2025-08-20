using System.ComponentModel.DataAnnotations;

namespace Vanfist.Entities;

public class Category
{
    public int Id { get; set; }

    [Required, StringLength(255)] public string Name { get; set; }

    [Required, StringLength(255)] public string Description { get; set; }

    public ICollection<Model> Models { get; set; } = new List<Model>();
}