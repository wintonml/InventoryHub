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
        policy.WithOrigins("http://localhost:5034", "https://localhost:7097")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("ClientAppPolicy");

app.MapGet(ApiEndpoints.Products, () =>
{
    return new Product []
    {
        new() { Id = 1, Name = "Laptop", Price = 1200.50, Stock = 25 },
        new() { Id = 2, Name = "Headphones", Price = 50.00, Stock = 100 }
    };
});

app.Run();