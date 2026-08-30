using System.ComponentModel.DataAnnotations;
using Shared.Models;
using Xunit;

namespace Tests;

public class ProductValidationTests
{
    [Fact]
    public void Product_WithValidData_ShouldPassValidation()
    {
        var product = new Product
        {
            Id = 1,
            Name = "Laptop",
            Price = 999.99,
            Stock = 10,
            Category = new Category
            {
                Id = 1,
                Name = "Electronics"
            }
        };

        var isValid = IsValid(product, out var results);

        Assert.True(isValid);
        Assert.Empty(results);
    }

    [Fact]
    public void Product_WithEmptyName_ShouldFailValidation()
    {
        var product = new Product
        {
            Id = 1,
            Name = string.Empty,
            Price = 100,
            Stock = 5,
            Category = new Category { Id = 1, Name = "Electronics" }
        };

        var isValid = IsValid(product, out var results);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Product.Name)));
    }

    [Fact]
    public void Product_WithNameLongerThan200Characters_ShouldFailValidation()
    {
        var product = new Product
        {
            Id = 2,
            Name = new string('A', 201),
            Price = 150.00,
            Stock = 8,
            Category = new Category { Id = 2, Name = "Accessories" }
        };

        var isValid = IsValid(product, out var results);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Product.Name)));
    }

    [Fact]
    public void Product_WithNegativePrice_ShouldFailValidation()
    {
        var product = new Product
        {
            Id = 3,
            Name = "Mouse",
            Price = -1,
            Stock = 20,
            Category = new Category { Id = 3, Name = "Accessories" }
        };

        var isValid = IsValid(product, out var results);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Product.Price)));
    }

    [Fact]
    public void Product_WithNegativeStock_ShouldFailValidation()
    {
        var product = new Product
        {
            Id = 4,
            Name = "Monitor",
            Price = 299.99,
            Stock = -1,
            Category = new Category { Id = 4, Name = "Electronics" }
        };

        var isValid = IsValid(product, out var results);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Product.Stock)));
    }

    [Fact]
    public void Product_WithNullCategory_ShouldFailValidation()
    {
        var product = new Product
        {
            Id = 5,
            Name = "Keyboard",
            Price = 25,
            Stock = 10,
            Category = null!
        };

        var isValid = IsValid(product, out var results);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Product.Category)));
    }

    private static bool IsValid(Product product, out List<ValidationResult> results)
    {
        var context = new ValidationContext(product);
        results = new List<ValidationResult>();

        return Validator.TryValidateObject(
            product,
            context,
            results,
            validateAllProperties: true);
    }
}
