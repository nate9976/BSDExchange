using BSDExchange.Enums;
using System.ComponentModel.DataAnnotations;

namespace BSDExchange.Controllers.Requests;

public record BestExecutionRequest
{
    [Required(ErrorMessage = "Required: Buy or Sell.")]
    [EnumDataType(typeof(OrderType), ErrorMessage = "Required: Buy or Sell.")]
    public OrderType? OrderType { get; init; }

    [Required(ErrorMessage = "Required: BTC amount greater than 0.")]
    [Range(typeof(decimal), "0", "79228162514264337593543950335", MinimumIsExclusive = true, ErrorMessage = "Must be greater than 0.")]
    public decimal? Amount { get; init; }
}
