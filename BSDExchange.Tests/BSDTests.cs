using BSDExchange.Configuration;
using BSDExchange.Enums;
using BSDExchange.Helpers;
using BSDExchange.Models;
using BSDExchange.Models.Exchange;
using BSDExchange.Models.OB;

namespace BSDExchange.Tests;

public class BSDTests
{
    private readonly Dictionary<string, ExchangeBalance> _balances = [];

    private OrderBook Exchange(string id, decimal eur, decimal btc, Order[]? bids = null, Order[]? asks = null)
    {
        _balances[id] = new ExchangeBalance(id, eur, btc);
        return new OrderBook(id, default, bids ?? [], asks ?? []);
    }

    private static Order Bid(decimal amount, decimal price) => new(null, default, "Buy", "Limit", amount, price);
    private static Order Ask(decimal amount, decimal price) => new(null, default, "Sell", "Limit", amount, price);

    private ExecutionPlan Buy(decimal amount, params OrderBook[] exchanges) =>
        MetaExchange.FindBestExecution(exchanges, _balances, OrderType.Buy, amount);

    private ExecutionPlan Sell(decimal amount, params OrderBook[] exchanges) =>
        MetaExchange.FindBestExecution(exchanges, _balances, OrderType.Sell, amount);

    private static void AssertOrders(ExecutionPlan plan, params (string Exchange, decimal Amount, decimal Price)[] expected) =>
        Assert.Equal(expected, plan.Orders.Select(o => (o.ExchangeId, o.Amount, o.Price)).ToArray());

    [Fact]
    public void Buy_TakesCheapestAsksAcrossExchanges()
    {
        var a = Exchange("a", eur: 1000, btc: 10, asks: [Ask(1, 103), Ask(1, 100)], bids: [Bid(1, 200), Bid(1, 170)]);
        var b = Exchange("b", eur: 1000, btc: 10, asks: [Ask(1, 101), Ask(1, 102)], bids: [Bid(1, 180), Bid(1, 190)]);

        var plan = Buy(3.5m, [a, b]);

        AssertOrders(plan, ("a", 1, 100), ("b", 1, 101), ("b", 1, 102), ("a", (decimal)0.5, 103));
        Assert.All(plan.Orders, o => Assert.Equal(OrderType.Buy, o.Type));
        Assert.Equal((decimal)354.5, plan.TotalEur);
        Assert.True(plan.IsFullyFilled);
    }

    [Fact]
    public void Sell_TakesHighestBids_LimitedByEachExchangesBtcBalance()
    {
        var a = Exchange("a", eur: 1000, btc: 10, asks: [Ask(1, 103), Ask(1, 100)], bids: [Bid(1, 200), Bid(1, 170)]);
        var b = Exchange("b", eur: 1000, btc: 10, asks: [Ask(1, 101), Ask(1, 102)], bids: [Bid(1, 180), Bid(1, 190)]);

        var plan = Sell((decimal)3.5, a, b);

        AssertOrders(plan, ("a", 1, 200), ("b", 1, 190), ("b", 1, 180), ("a", (decimal)0.5, 170));
        Assert.All(plan.Orders, o => Assert.Equal(OrderType.Sell, o.Type));
        Assert.Equal((decimal)655, plan.TotalEur);
        Assert.True(plan.IsFullyFilled);
    }

    [Fact]
    public void NotEnoughLiquidityOrBalance_FillsWhatIsAvailable()
    {
        var funded = Exchange("funded", eur: 1000, btc: 0, asks: [Ask(10, 50), Ask(1, 1), Ask(1, 100)]);
        var notEnoughFunds = Exchange("notEnoughFunds", eur: 1, btc: 0, asks: [Ask(10, 2)]);

        var plan = Buy(5, funded, notEnoughFunds);

        AssertOrders(plan, ("funded", 1, 1), ("notEnoughFunds", (decimal)0.5, 2), ("funded", (decimal)3.5, 50));
        Assert.Equal((decimal)177, plan.TotalEur);
        Assert.True(plan.IsFullyFilled);
    }

    [Fact]
    public void NegativeAmount_PrintsUsageAndReturnsError()
    {
        int exitCode = ConsoleApp.Run(["buy", "-1"], new DataFilesOptions());

        Assert.Equal(1, exitCode);
    }
}