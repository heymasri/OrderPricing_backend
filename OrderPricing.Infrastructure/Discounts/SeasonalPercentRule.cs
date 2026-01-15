using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using OrderPricing.Domain.Abstractions;
using OrderPricing.Domain.Entities;
using OrderPricing.Domain.ValueObjects;

namespace OrderPricing.Infrastructure.Discounts;

public sealed class SeasonalPercentRule : IDiscountRule
{
    public string Name => "Seasonal 10% (Nov-Dec)";
    private readonly decimal _percent = 0.10m; //m=decimal literal

    public bool IsApplicable(Order order)
        => order.CreatedAt.Month is 11 or 12;

    public Money Apply(Money currentTotal, Order order)
        => currentTotal.Multiply(1 - _percent);//1 - _percent = 1 - 0.10 = 0.90
}

