using BSDExchange.Controllers.Requests;
using BSDExchange.Helpers;
using BSDExchange.Models;
using BSDExchange.Models.Exchange;
using Microsoft.AspNetCore.Mvc;

namespace BSDExchange.Controllers;

[ApiController]
[Route("api/best-execution")]
public class BestExecutionController(ExchangeData data) : ControllerBase
{
    [HttpPost]
    [EndpointDescription("1 - Buy, 2 - Sell")]
    [ProducesResponseType<ExecutionPlan>(StatusCodes.Status200OK, "application/json")]
    public ActionResult<ExecutionPlan> Post(BestExecutionRequest request) =>
        MetaExchange.FindBestExecution(data.OrderBooks, data.Balances, request.OrderType!.Value, request.Amount!.Value);
}
