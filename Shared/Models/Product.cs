using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public double Price { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    [Required]
    public Category Category { get; set; } = new();
}
