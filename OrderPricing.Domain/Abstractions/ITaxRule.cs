using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// src/OrderPricing.Domain/Abstractions/ITaxRule.cs
using OrderPricing.Domain.Entities;
using OrderPricing.Domain.ValueObjects;

namespace OrderPricing.Domain.Abstractions;

public interface ITaxRule
{
    string Name { get; }
    Money Compute(Money taxableAmount, Order order);
}

