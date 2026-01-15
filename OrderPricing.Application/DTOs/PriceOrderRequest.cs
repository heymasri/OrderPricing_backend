using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using OrderPricing.Domain.Entities;
using OrderPricing.Domain.ValueObjects;

namespace OrderPricing.Application.DTOs;
//what the client sends to the API - to caculate the price
public sealed class PriceOrderRequest
{
    public string Region { get; init; } = "IN";
    public string Currency { get; init; } = "INR";
    public string? CouponCode { get; init; }
    public string CustomerTier { get; init; } = "Regular";
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public List<PriceOrderItem> Items { get; init; } = new();
    // Each item has SKU,name, unit price, quantity.
}

public sealed class PriceOrderItem
{
    public string Sku { get; init; } = default!;//SKU- Unique product code/
    public string Name { get; init; } = default!;
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
}

public static class RequestMapping
{
    public static Order ToDomain(this PriceOrderRequest req)
    {
        var tier = Enum.TryParse<CustomerTier>(req.CustomerTier, true, out var t) ? t : CustomerTier.Regular;

        var items = req.Items.Select(i =>
            new LineItem(i.Sku, i.Name, new Money(i.UnitPrice, req.Currency), i.Quantity));

        return new Order(tier, req.Region, req.CreatedAt, items, req.CouponCode);
    }
    //Bridges API Layer -> Domain Layer
}

