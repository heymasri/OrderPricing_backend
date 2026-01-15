using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using OrderPricing.Domain.Abstractions;
using OrderPricing.Domain.Entities;
using OrderPricing.Domain.ValueObjects;

namespace OrderPricing.Infrastructure.Discounts;

public sealed class LoyaltyTierRule : IDiscountRule
{
    public string Name => "Loyalty Tier Discount";
    public bool IsApplicable(Order order) => order.Tier is CustomerTier.Gold or CustomerTier.Platinum;

    public Money Apply(Money currentTotal, Order order)
    {
        var percent = order.Tier switch
        {
            CustomerTier.Gold => 0.05m,
            CustomerTier.Platinum => 0.08m,
            _ => 0m //tier is not Gold/Platinum → no discount.
        };
        return percent == 0m ? currentTotal : currentTotal.Multiply(1 - percent);
    }
}

