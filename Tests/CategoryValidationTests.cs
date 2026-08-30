using System.ComponentModel.DataAnnotations;
using Shared.Models;
using Xunit;

namespace Tests;

public class CategoryValidationTests
{
    [Fact]
    public void Category_WithValidData_ShouldPassValidation()
    {
        var category = new Category
        {
            Id = 1,
            Name = "Electronics"
        };

        var isValid = IsValid(category, out var results);

        Assert.True(isValid);
        Assert.Empty(results);
    }

    [Fact]
    public void Category_WithEmptyName_ShouldFailValidation()
    {
        var category = new Category
        {
            Id = 1,
            Name = string.Empty
        };

        var isValid = IsValid(category, out var results);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Category.Name)));
    }

    [Fact]
    public void Category_WithNameLongerThan100Characters_ShouldFailValidation()
    {
        var category = new Category
        {
            Id = 2,
            Name = new string('A', 101)
        };

        var isValid = IsValid(category, out var results);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Category.Name)));
    }

    [Fact]
    public void Category_WithWhitespaceName_ShouldFailValidation()
    {
        var category = new Category
        {
            Id = 3,
            Name = "   "
        };

        var isValid = IsValid(category, out var results);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Category.Name)));
    }

    private static bool IsValid(Category category, out List<ValidationResult> results)
    {
        var context = new ValidationContext(category);
        results = new List<ValidationResult>();

        return Validator.TryValidateObject(
            category,
            context,
            results,
            validateAllProperties: true);
    }
}
