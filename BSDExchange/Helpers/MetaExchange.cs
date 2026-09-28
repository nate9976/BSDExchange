using BSDExchange.Enums;
using BSDExchange.Models;
using BSDExchange.Models.Exchange;
using BSDExchange.Models.OB;

namespace BSDExchange.Helpers;

public static class MetaExchange
{
    public const int BtcDecimals = 8;

    public static ExecutionPlan FindBestExecution(
        IReadOnlyList<OrderBook> exchanges,
        IReadOnlyDictionary<string, ExchangeBalance> balances,
        OrderType type,
        decimal amount)
    {
        ValidateInput(exchanges, balances, amount);

        var levels = PriceLevelsBestFirst(exchanges, type);
        var balanceLeft = StartingBalances(exchanges, balances, type);
        var orders = new List<HedgeOrder>();
        decimal remaining = amount;

        foreach (var level in levels)
        {
            if (remaining == 0)
                break;

            decimal quantity = QuantityToTake(level, remaining, balanceLeft[level.ExchangeId], type);
            if (quantity <= 0)
                continue;

            orders.Add(new HedgeOrder(level.ExchangeId.ToString(), type, quantity, level.Price));
            balanceLeft[level.ExchangeId] -= BalanceUsed(quantity, level.Price, type);
            remaining -= quantity;
        }

        return new ExecutionPlan(type, amount, orders);
    }

    private static void ValidateInput(
        IReadOnlyList<OrderBook> exchanges,
        IReadOnlyDictionary<string, ExchangeBalance> balances,
        decimal amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        var missing = exchanges.FirstOrDefault(e => !balances.ContainsKey(e.ExchangeId));
        if (missing is not null)
            throw new ArgumentException($"No balance for the order book with ExchangeId {missing.ExchangeId}.", nameof(balances));
    }

    private static IEnumerable<PriceLevel> PriceLevelsBestFirst(IReadOnlyList<OrderBook> exchanges, OrderType type)
    {
        var levels = exchanges.SelectMany((exchange, index) =>
            OrdersToTake(exchange, type)
                .Where(o => o.Amount > 0 && o.Price > 0)
                .Select(o => new PriceLevel(exchange.ExchangeId, o.Amount, o.Price)));

        return type == OrderType.Buy
            ? levels.OrderBy(l => l.Price)
            : levels.OrderByDescending(l => l.Price);
    }

    private static IReadOnlyList<Order> OrdersToTake(OrderBook exchange, OrderType type) =>
        type == OrderType.Buy ? exchange.Asks : exchange.Bids;

    private static Dictionary<string, decimal> StartingBalances(
        IReadOnlyList<OrderBook> exchanges,
        IReadOnlyDictionary<string, ExchangeBalance> balances,
        OrderType type) =>
        exchanges
            .Select(e => e.ExchangeId)
            .Distinct()
            .ToDictionary(t => t, t => Math.Max(0, BalanceToSpend(balances[t], type)));

    private static decimal BalanceToSpend(ExchangeBalance balance, OrderType type) =>
        type == OrderType.Buy ? balance.EurBalance : balance.BtcBalance;

    private static decimal QuantityToTake(PriceLevel level, decimal remaining, decimal balanceLeft, OrderType type)
    {
        decimal affordable = type == OrderType.Buy
            ? Math.Round(balanceLeft / level.Price, BtcDecimals, MidpointRounding.ToZero)
            : balanceLeft;

        return Math.Min(remaining, Math.Min(level.Amount, affordable));
    }

    private static decimal BalanceUsed(decimal quantity, decimal price, OrderType type) =>
        type == OrderType.Buy ? quantity * price : quantity;
}
