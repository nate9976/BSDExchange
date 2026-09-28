namespace BSDExchange.Configuration;

public record DataFilesOptions
{
    public string OrderBooksPath { get; init; } = "";
    public string BalancesPath { get; init; } = "";
}
