using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OrderPricing.Application.DTOs;

//what API sends back to the client after pricing is calculated
public sealed class PriceOrderResponse
{
    public string Currency { get; init; } = "INR";
    public decimal Subtotal { get; init; } //total before discount, tax
    public decimal DiscountTotal { get; init; }
    public decimal Tax { get; init; }
    public decimal GrandTotal { get; init; } // total after dsicount, tax

    public List<BreakdownLine> Breakdown { get; init; } = new(); // breakdown of discount rule aaplied
}

public sealed class BreakdownLine
{
    public string Name { get; init; } = default!;
    public decimal Amount { get; init; }
}

