using Microsoft.Extensions.Caching.Memory;
using Shared.Constants;
using Shared.Models;

var builder = WebApplication.CreateBuilder(args);

ConfigureJsonOptions(builder.Services);
builder.Services.AddMemoryCache();
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
app.MapGet(ApiEndpoints.ProductList, (HttpContext httpContext, IMemoryCache cache) =>
{
    httpContext.Response.Headers.CacheControl = "public, max-age=300";

    var products = cache.GetOrCreate(CacheKeys.ProductCatalog, entry =>
    {
        entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
        entry.SetSlidingExpiration(TimeSpan.FromMinutes(2));
        return GetProducts();
    });

    return Results.Ok(products);
});

app.Run();

static void ConfigureJsonOptions(IServiceCollection services)
{
    services.ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.PropertyNamingPolicy = null;
        options.SerializerOptions.PropertyNameCaseInsensitive = true;
    });
}

static Product[] GetProducts() =>
[
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
];