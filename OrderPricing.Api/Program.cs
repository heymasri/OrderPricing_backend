
using OrderPricing.Application.DTOs;
using OrderPricing.Application.Services;
using OrderPricing.Domain.Abstractions;
using OrderPricing.Infrastructure.Discounts;
using OrderPricing.Infrastructure.Taxes;

var builder = WebApplication.CreateBuilder(args);

// DI registrations
builder.Services.AddSingleton<IDiscountRule, SeasonalPercentRule>();
builder.Services.AddSingleton<IDiscountRule, LoyaltyTierRule>();
builder.Services.AddSingleton<IDiscountRule, FixedCouponRule>();
builder.Services.AddSingleton<IDiscountRule, BogoRule>();
builder.Services.AddSingleton<ITaxRule, RegionTaxRule>();
builder.Services.AddSingleton<PricingService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Health + Root
app.MapGet("/", () => "Order Pricing API is running. Try GET /health or POST /price.");
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));


//main endpoint /price.PriceOrderRequest (the input from the client),PricingService (the service that calculates the price)
app.MapPost("/price", (PriceOrderRequest req, PricingService svc) =>
{
    var res = svc.Price(req);
    return Results.Ok(res);
});

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
