namespace BSDExchange.Models.OB;

public record RawOrderBook(DateTime AcqTime, List<RawOrderEntry> Bids, List<RawOrderEntry> Asks);