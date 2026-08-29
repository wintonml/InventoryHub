using System.ComponentModel.DataAnnotations;
using Shared.Models;
using Xunit;

namespace Shared.Tests;

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

        var context = new ValidationContext(product);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(
            product,
            context,
            results,
            validateAllProperties: true);

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
            Category = new Category
            {
                Id = 1,
                Name = "Electronics"
            }
        };

        var context = new ValidationContext(product);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(
            product,
            context,
            results,
            validateAllProperties: true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Product.Name)));
    }

    [Fact]
    public void Product_WithNullCategory_ShouldFailValidation()
    {
        var product = new Product
        {
            Id = 1,
            Name = "Keyboard",
            Price = 25,
            Stock = 10,
            Category = null!
        };

        var context = new ValidationContext(product);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(
            product,
            context,
            results,
            validateAllProperties: true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Product.Category)));
    }
}
