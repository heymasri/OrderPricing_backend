using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// src/OrderPricing.Domain/Abstractions/IDiscountRule.cs
using OrderPricing.Domain.Entities;
using OrderPricing.Domain.ValueObjects;

namespace OrderPricing.Domain.Abstractions;

public interface IDiscountRule
{
    string Name { get; }
    bool IsApplicable(Order order);
    Money Apply(Money currentTotal, Order order);
}

