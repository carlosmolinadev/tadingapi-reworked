using Microsoft.AspNetCore.Mvc;
using TradingApp.Application.Features.Services.Interfaces;
using TradingApp.Application.Models.Requests;
using TradingApp.Application.Models.Responses;

namespace TradingApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TradeController : ControllerBase
{
    private readonly ILogger<TradeController> _logger;
    private readonly ITradeService _tradeService;
    public TradeController(ILogger<TradeController> logger, ITradeService tradeService)
    {
        _logger = logger;
        _tradeService = tradeService;
    }

    [HttpPost("AddTrade")]
    public async Task<ActionResult> AddTrade([FromBody] AddTradeRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var response = await _tradeService.AddTrade(request);

        if (response.Success)
        {
            return Ok(response);
        }

        return BadRequest(response);

    }
}
