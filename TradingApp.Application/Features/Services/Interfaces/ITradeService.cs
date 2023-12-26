using TradingApp.Application.Models.Requests;
using TradingApp.Application.Models.Responses;
using TradingApp.Domain.Entities;

namespace TradingApp.Application.Features.Services.Interfaces
{
    public interface ITradeService
    {
        public Task<AddTradeResponse> AddTrade(AddTradeRequest request);
    }
}