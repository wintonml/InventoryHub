using Shared.Constants;
using Shared.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = null;
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientAppPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("ClientAppPolicy");

app.MapGet(ApiEndpoints.ProductList, () =>
{
    return new Product[]
    {
        new()
        {
            Id = 1,
            Name = "Laptop",
            Price = 1200.50,
            Stock = 25,
            Category = new Category { Id = 1, Name = "Electronics" }
        },
        new()
        {
            Id = 2,
            Name = "Headphones",
            Price = 50.00,
            Stock = 100,
            Category = new Category { Id = 2, Name = "Accessories" }
        }
    };
});

app.Run();