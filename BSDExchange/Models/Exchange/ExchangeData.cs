using BSDExchange.Models.OB;

namespace BSDExchange.Models.Exchange;

public record ExchangeData(IReadOnlyList<OrderBook> OrderBooks, IReadOnlyDictionary<string, ExchangeBalance> Balances);