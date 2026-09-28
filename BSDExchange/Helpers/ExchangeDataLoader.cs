using BSDExchange.Models.Exchange;
using BSDExchange.Models.OB;
using BSDExchange.Validation;
using System.Text.Json;

namespace BSDExchange.Helpers;

public static class ExchangeDataLoader
{
    public static ExchangeData Load(string orderBooksPath, string balancesPath)
    {
        foreach (var path in new[] { orderBooksPath, balancesPath })
        {
            if (!File.Exists(path))
                throw new InvalidDataException($"data file not found at '{Path.GetFullPath(path)}'.");
        }

        List<ExchangeBalance> balanceList;
        try
        {
            balanceList = JsonSerializer.Deserialize<List<ExchangeBalance>>(File.ReadAllText(balancesPath)) ?? [];
        }
        catch (Exception ex) when (ex is JsonException or IOException or UnauthorizedAccessException)
        {
            throw new InvalidDataException($"invalid {balancesPath} ({ex.Message}).", ex);
        }

        string? balancesError = BalancesFileValidator.Validate(balanceList);
        if (balancesError is not null)
            throw new InvalidDataException($"{balancesPath} {balancesError}");
        var balances = balanceList.ToDictionary(b => b.ExchangeId);

        IReadOnlyList<OrderBook> orderBooks;
        try
        {
            orderBooks = OrderBookParser.ParseFile(orderBooksPath).OrderBooks;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            throw new InvalidDataException($"cannot read {orderBooksPath} ({ex.Message}).", ex);
        }
        string? orderBooksError = OrderBooksFileValidator.Validate(orderBooks, balances);
        if (orderBooksError is not null)
            throw new InvalidDataException($"{orderBooksPath} {orderBooksError}");

        return new ExchangeData(orderBooks, balances);
    }
}
