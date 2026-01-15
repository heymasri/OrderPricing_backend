using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// src/OrderPricing.Domain/Entities/LineItem.cs
using OrderPricing.Domain.ValueObjects;

namespace OrderPricing.Domain.Entities;

public sealed class LineItem
{
    public string Sku { get; }
    public string Name { get; }
    public Money UnitPrice { get; }
    public int Quantity { get; }

    // stock keeping unit - SKUs starting with "BOGO-" are eligible for Buy-One-Get-One.
    public bool IsBogoEligible => Sku.StartsWith("BOGO-", StringComparison.OrdinalIgnoreCase);

    public LineItem(string sku, string name, Money unitPrice, int quantity)
    {
        if (string.IsNullOrWhiteSpace(sku)) throw new ArgumentException("SKU required");
        if (quantity <= 0) throw new ArgumentException("Quantity must be > 0");
        Sku = sku; Name = name; UnitPrice = unitPrice; Quantity = quantity;
    }

    public Money Total() => UnitPrice.Multiply(Quantity);
}

