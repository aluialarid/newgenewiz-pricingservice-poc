var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Sample pricing data
var pricingDatabase = new Dictionary<int, PricingInfo>
{
    { 1, new(1, "Product A", 150.50m, 100) },
    { 2, new(2, "Product B", 250.75m, 50) },
    { 3, new(3, "Product C", 320.00m, 75) },
    { 4, new(4, "Product D", 180.25m, 200) },
};

// GET /pricing/{itemId} - Returns pricing information for a specific item
app.MapGet("/pricing/{itemId}", (int itemId) =>
{
    if (pricingDatabase.TryGetValue(itemId, out var pricing))
    {
        return Results.Ok(pricing);
    }

    return Results.NotFound(new { message = $"No pricing found for item {itemId}" });
})
.WithName("GetPricing");

await app.RunAsync();

record PricingInfo(int ItemId, string ProductName, decimal Price, int StockQuantity);

