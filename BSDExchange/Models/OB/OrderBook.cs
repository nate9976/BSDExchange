namespace BSDExchange.Models.OB;

public record OrderBook(string ExchangeId, DateTime AcqTime, IReadOnlyList<Order> Bids, IReadOnlyList<Order> Asks);