using BSDExchange.Enums;

namespace BSDExchange.Models;

public record ExecutionPlan(OrderType Type, decimal RequestedAmount, IReadOnlyList<HedgeOrder> Orders)
{
    public decimal FilledAmount => Orders.Sum(o => o.Amount);

    public decimal TotalEur => Orders.Sum(o => o.Total);

    public bool IsFullyFilled => FilledAmount == RequestedAmount;
}