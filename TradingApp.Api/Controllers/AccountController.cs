using Microsoft.AspNetCore.Mvc;
using TradingApp.Application.Models.Responses;
using TradingApp.Domain.Entities;

namespace TradingApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;

    public AccountController(ILogger<AccountController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "GetAccount/{accountId}")]
    public async Task<ActionResult> Get()
    {
        return Ok();
    }
}
