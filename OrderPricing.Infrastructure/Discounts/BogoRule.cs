using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using OrderPricing.Domain.Abstractions;
using OrderPricing.Domain.Entities;
using OrderPricing.Domain.ValueObjects;

namespace OrderPricing.Infrastructure.Discounts;

// Buy-One-Get-One: For BOGO- SKUs, every second item is free.
public sealed class BogoRule : IDiscountRule
{
    public string Name => "BOGO";

    public bool IsApplicable(Order order)
        => order.Items.Any(i => i.IsBogoEligible && i.Quantity > 1);

    public Money Apply(Money currentTotal, Order order)
    {
        var currency = currentTotal.Currency;
        var discount = Money.Zero(currency);

        foreach (var item in order.Items.Where(i => i.IsBogoEligible))
        {
            var freeUnits = item.Quantity / 2;  // integer division: 2→1 free, 3→1 free, 4→2 free
            var valueOff = item.UnitPrice.Multiply(freeUnits);
            discount = discount.Add(valueOff);
        }

        var after = currentTotal.Subtract(discount);
        return after.Amount < 0 ? Money.Zero(currency) : after;//inline if else statement
        //condition ? value if true: value if false
    }
}

