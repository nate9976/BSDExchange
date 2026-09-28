namespace BSDExchange.Models.OB;
public record Order(string? Id, DateTime Time, string Type, string Kind, decimal Amount, decimal Price);
