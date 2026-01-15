using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// src/OrderPricing.Domain/Entities/Order.cs
using OrderPricing.Domain.ValueObjects;

namespace OrderPricing.Domain.Entities;

public enum CustomerTier { Regular = 0, Silver = 1, Gold = 2, Platinum = 3 }

public sealed class Order
{
    private readonly List<LineItem> _items = new();

    public IReadOnlyList<LineItem> Items => _items;
    public CustomerTier Tier { get; }
    public DateTimeOffset CreatedAt { get; }
    public string Region { get; }  // e.g., "IN", "US", "EU"
    public string? CouponCode { get; }

    //IEnumerable - interface, iterate over elements in a collections using for each loop.
    //String? - nullable reference type.
    public Order(CustomerTier tier, string region, DateTimeOffset createdAt, IEnumerable<LineItem> items, string? couponCode = null)
    {
        if (string.IsNullOrWhiteSpace(region)) throw new ArgumentException("Region required");
        Tier = tier;
        Region = region.ToUpperInvariant();
        CreatedAt = createdAt;
        _items.AddRange(items ?? Enumerable.Empty<LineItem>());
        CouponCode = couponCode;
    }

    public Money Subtotal(string currency = "INR")
        => _items.Select(i => i.Total()).Aggregate(Money.Zero(currency), (acc, m) => acc.Add(m));
}

