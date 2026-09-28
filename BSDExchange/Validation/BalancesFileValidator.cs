using BSDExchange.Models.Exchange;

namespace BSDExchange.Validation;

public static class BalancesFileValidator
{
    public static string? Validate(IReadOnlyList<ExchangeBalance?> balances)
    {
        if (balances.Any(b => b is null))
            return "contains a null entry.";

        var negative = balances.FirstOrDefault(b => b!.EurBalance < 0 || b.BtcBalance < 0);
        if (negative is not null)
            return $"has a negative balance for ExchangeId {negative.ExchangeId}.";

        var duplicate = balances.GroupBy(b => b!.ExchangeId).FirstOrDefault(g => g.Count() > 1);
        if (duplicate is not null)
            return $"has more than one entry for ExchangeId {duplicate.Key}.";

        return null;
    }
}
