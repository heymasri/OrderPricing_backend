using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using OrderPricing.Domain.Abstractions;
using OrderPricing.Domain.Entities;
using OrderPricing.Domain.ValueObjects;

namespace OrderPricing.Infrastructure.Taxes;

public sealed class RegionTaxRule : ITaxRule
{
    public string Name => "Region Tax";

    private readonly IReadOnlyDictionary<string, decimal> _rates = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
    {
        ["IN"] = 0.18m, // GST 18% example
        ["US"] = 0.07m,
        ["EU"] = 0.20m
    };

    public Money Compute(Money taxableAmount, Order order)
    {
        var rate = _rates.TryGetValue(order.Region, out var r) ? r : 0.00m;
        return taxableAmount.Percent(rate);
    }
}

