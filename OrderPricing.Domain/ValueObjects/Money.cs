using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OrderPricing.Domain.ValueObjects;

//sealed- No Inheritance from this class , IEquatable-> equality checks.
public sealed class Money : IEquatable<Money>
{
    //Amount -> numeric value.
    //Currency ->"INR" or "USD"
    public decimal Amount { get; }
    public string Currency { get; }

    //constructor money
    public Money(decimal amount, string currency = "INR")
    {
        if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency required");
        Amount = decimal.Round(amount, 2, MidpointRounding.ToEven);
        Currency = currency.ToUpperInvariant();
    }
    
    public static Money Zero(string currency = "INR") => new(0m, currency);
    
    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount - other.Amount, Currency);
    }

    public Money Multiply(decimal factor) => new(Amount * factor, Currency);

    public Money Percent(decimal percent) => new(Amount * percent, Currency);

    private void EnsureSameCurrency(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Currency mismatch");
    }

    public bool Equals(Money? other) => other is not null && Amount == other.Amount && Currency == other.Currency;
    public override bool Equals(object? obj) => obj is Money m && Equals(m);
    public override int GetHashCode() => HashCode.Combine(Amount, Currency);
    public override string ToString() => $"{Currency} {Amount:0.00}";
}

