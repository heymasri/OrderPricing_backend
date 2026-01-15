using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using OrderPricing.Domain.Abstractions;
using OrderPricing.Domain.Entities;
using OrderPricing.Domain.ValueObjects;

namespace OrderPricing.Infrastructure.Discounts;

public sealed class FixedCouponRule : IDiscountRule
{
    public string Name => "Fixed Coupon";
    private readonly Dictionary<string, decimal> _coupons = new(StringComparer.OrdinalIgnoreCase)
    {
        ["SAVE50"] = 50m,
        ["SAVE100"] = 100m
    };

    public bool IsApplicable(Order order)
        => order.CouponCode is not null && _coupons.ContainsKey(order.CouponCode!);
    //Applies only if: Coupon code is provided & exists in dictionary
    public Money Apply(Money currentTotal, Order order)
    {
        var value = new Money(_coupons[order.CouponCode!], currentTotal.Currency);
        var after = currentTotal.Subtract(value);
        return after.Amount < 0 ? Money.Zero(currentTotal.Currency) : after;
    }
}

