using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using OrderPricing.Application.DTOs;
using OrderPricing.Application.Services;
using OrderPricing.Domain.Abstractions;
using OrderPricing.Infrastructure.Discounts;
using OrderPricing.Infrastructure.Taxes;
using Xunit;

public class PricingServiceTests
{
    private PricingService CreateService()
    {
        var discounts = new IDiscountRule[]
        {
            new SeasonalPercentRule(),
            new LoyaltyTierRule(),
            new FixedCouponRule(),
            new BogoRule()
        };
        var tax = new RegionTaxRule();
        return new PricingService(discounts, tax);
    }

    [Fact]
    public void Applies_Seasonal_And_Tier_Discounts_Then_Tax()
    {
        var svc = CreateService();
        var req = new PriceOrderRequest
        {
            Region = "IN",
            Currency = "INR",
            CustomerTier = "Gold",
            CreatedAt = new DateTimeOffset(2025, 12, 10, 0, 0, 0, TimeSpan.Zero),
            Items = new()
            {
                new PriceOrderItem { Sku = "SKU-1", Name = "Item", UnitPrice = 1000m, Quantity = 1 }
            }
        };

        var res = svc.Price(req);

        Assert.Equal(1000m, res.Subtotal);
        Assert.True(res.DiscountTotal > 0m);   // seasonal + tier
        Assert.True(res.Tax > 0m);             // tax on discounted amount
        Assert.True(res.GrandTotal > 0m);
    }

    [Fact]
    public void Bogo_Applies_Free_Items()
    {
        var svc = CreateService();
        var req = new PriceOrderRequest
        {
            Region = "US",
            Currency = "USD",
            CustomerTier = "Regular",
            CreatedAt = DateTimeOffset.UtcNow,
            Items = new()
            {
                new PriceOrderItem { Sku = "BOGO-XYZ", Name = "Bogo Item", UnitPrice = 50m, Quantity = 3 } // 1 free
            }
        };

        var res = svc.Price(req);

        Assert.Equal(150m, res.Subtotal);      // 3 * 50
        Assert.True(res.DiscountTotal >= 50m); // one unit off
    }
}

