using BSDExchange.Enums;

namespace BSDExchange.Models;

public record HedgeOrder(string ExchangeId, OrderType Type, decimal Amount, decimal Price)
{
    public decimal Total => Amount * Price;
}
