using BSDExchange.Configuration;
using BSDExchange.Enums;
using BSDExchange.Helpers;
using BSDExchange.Models.Exchange;

namespace BSDExchange;

public class ConsoleApp
{
    public static int Run(string[] args, DataFilesOptions dataFiles)
    {
        // Validate arguments
        OrderType? parsedType = args.Length > 0
            ? args[0] switch { "buy" => OrderType.Buy, "sell" => OrderType.Sell, _ => null }
            : null;

        if (args.Length != 2
            || parsedType == null
            || !decimal.TryParse(args[1], out var amount)
            || amount <= 0)
        {
            Console.WriteLine("Usage: BSDExample <buy|sell> <amount in BTC>   e.g. BSDExample buy 1.5");
            Console.WriteLine("       BSDExample (no arguments: start the web API with Swagger UI)");
            return 1;
        }

        // Parse exchange data
        ExchangeData data;
        try
        {
            data = ExchangeDataLoader.Load(dataFiles.OrderBooksPath, dataFiles.BalancesPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return 1;
        }

        // Find best execution plan
        var plan = MetaExchange.FindBestExecution(data.OrderBooks, data.Balances, (OrderType)parsedType!, amount);

        // Print results to console
        Console.WriteLine($"{parsedType} {amount} BTC across {data.OrderBooks.Count} exchanges");
        Console.WriteLine("Orders:");
        foreach (var o in plan.Orders)
        {
            Console.WriteLine($"  {o.ExchangeId,-16} {o.Type,-4} {o.Amount,-12} BTC @ {o.Price,10} EUR = {Math.Round(o.Total, 2),10} EUR");
        }

        Console.WriteLine($"Filled {plan.FilledAmount} of {amount} BTC, {(parsedType == OrderType.Buy ? "total cost" : "total proceeds")} {Math.Round(plan.TotalEur, 2)} EUR");

        if (!plan.IsFullyFilled)
        {
            Console.WriteLine("Warning: not enough liquidity or balance to fill the full amount.");
            return 2;
        }

        return 0;
    }
}
