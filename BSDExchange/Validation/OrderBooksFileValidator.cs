using BSDExchange.Models.Exchange;
using BSDExchange.Models.OB;

namespace BSDExchange.Validation;

public static class OrderBooksFileValidator
{
    public static string? Validate(IReadOnlyList<OrderBook> orderBooks, IReadOnlyDictionary<string, ExchangeBalance> balances)
    {
        if (orderBooks.Count == 0)
            return "contains no valid order books.";

        var unmatched = orderBooks.Where(b => !balances.ContainsKey(b.ExchangeId)).ToList();
        if (unmatched.Count > 0)
            return $"has {unmatched.Count} order book(s) without a balance, e.g. ExchangeId {unmatched[0].ExchangeId}.";

        return null;
    }
}
