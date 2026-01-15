using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using OrderPricing.Application.DTOs;
using OrderPricing.Domain.Abstractions;
using OrderPricing.Domain.ValueObjects;

namespace OrderPricing.Application.Services;

//actual price calucaltion
public sealed class PricingService
{
    private readonly IEnumerable<IDiscountRule> _discounts; // list of all discount rules (Seasonal, Loyalty, Coupon, BOGO).
    private readonly ITaxRule _taxRule; //tax rule (Region-based tax).
                                  

    // Constructor: DI injects discount rules and tax rule
    public PricingService(IEnumerable<IDiscountRule> discounts, ITaxRule taxRule)
    {
        _discounts = discounts;
        _taxRule = taxRule;
    }

    //takes PriceOrderRequest (input from client) and returns PriceOrderResponse (output).
    public PriceOrderResponse Price(PriceOrderRequest request)
    {
        var order = request.ToDomain(); //Converts the raw API request into an Order object
        var currency = request.Currency;

        var subtotal = order.Subtotal(currency);
        var discountTotal = Money.Zero(currency);
        var breakdown = new List<BreakdownLine>();

        var running = subtotal; // current total after applying discounts
        //apply discounts
        foreach (var rule in _discounts.Where(d => d.IsApplicable(order)))
        {
            var before = running; //Save the current total before applying the rule.
            running = rule.Apply(running, order); //apply discount rule to current total
            var diff = before.Subtract(running); // calculate how much is discounted
            if (diff.Amount > 0)
            {
                discountTotal = discountTotal.Add(diff);
                breakdown.Add(new BreakdownLine { Name = rule.Name, Amount = diff.Amount });
            }
            //After this loop, running = subtotal minus all discounts.
        }

        var tax = _taxRule.Compute(running, order);
        var grand = running.Add(tax);

        //Build respone DTO
        return new PriceOrderResponse
        {
            Currency = currency,
            Subtotal = subtotal.Amount,
            DiscountTotal = discountTotal.Amount,
            Tax = tax.Amount,
            GrandTotal = grand.Amount,
            Breakdown = breakdown
        };
        //API sends back  to the client
    }
}


