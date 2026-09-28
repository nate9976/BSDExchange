using BSDExchange.Models.OB;
using System.Text.Json;

namespace BSDExchange.Helpers;

public static class OrderBookParser
{
    public static OrderBookData ParseFile(string path)
    {
        var orderBooks = new List<OrderBook>();
        int lineNumber = 0;

        foreach (var line in File.ReadLines(path))
        {
            lineNumber++;
            if (string.IsNullOrWhiteSpace(line))
                continue;

            int tab = line.IndexOf('\t');
            if (tab < 0)
            {
                Console.WriteLine($"Line {lineNumber}: missing tab separator, skipped.");
                continue;
            }

            var ExchangeId = line.AsSpan(0, tab).ToString();
            RawOrderBook? raw;
            try
            {
                raw = JsonSerializer.Deserialize<RawOrderBook>(line.AsSpan(tab + 1));
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Line {lineNumber}: invalid JSON ({ex.Message}), skipped.");
                continue;
            }
            if (raw is null)
            {
                Console.WriteLine($"Line {lineNumber}: empty order book, skipped.");
                continue;
            }

            orderBooks.Add(new OrderBook(
                ExchangeId,
                raw.AcqTime,
                raw.Bids.Select(e => e.Order).ToList(),
                raw.Asks.Select(e => e.Order).ToList()));
        }

        return new OrderBookData(orderBooks);
    }
}