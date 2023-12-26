using TradingApp.Application.Features.Services.Interfaces;
using TradingApp.Application.Models.Requests;
using TradingApp.Application.Models.Responses;
using TradingApp.Application.Repositories;
using TradingApp.Application.Repositories.Base;
using TradingApp.Domain.Entities;

namespace TradingApp.Application.Features.TradeFeatures;

public class TradeService : ITradeService
{
    private readonly IRepository<Trade> _tradeRepository;
    private readonly IRepository<Account> _accountRepository;
    private readonly IReadRepository<Symbol> _symbolRepository;
    private readonly IReadRepository<Derivate> _derivateRepository;
    private readonly IReadRepository<DefaultTradeParameter> _defaultTradeParameter;

    public TradeService(IReadRepository<Symbol> symbolRepository, IReadRepository<DefaultTradeParameter> defaultConditionalRepository, IRepository<Account> accountRepository, IRepository<Trade> tradeRepository, IReadRepository<Derivate> derivateRepository)
    {
        _tradeRepository = tradeRepository;
        _accountRepository = accountRepository;
        _symbolRepository = symbolRepository;
        _defaultTradeParameter = defaultConditionalRepository;
        _derivateRepository = derivateRepository;
    }

    public async Task<AddTradeResponse> AddTrade(AddTradeRequest request)
    {
        var response = new AddTradeResponse();
        try
        {
            var account = await _accountRepository.SelectByIdAsync(request.AccountId);
            if (account is null)
            {
                response.SetMessage("Account was not found");
                return response;
            }

            var accountDerivate = await _derivateRepository.SelectByIdAsync(account.DerivateId);
            if (accountDerivate == null)
            {
                response.SetMessage("Account Derivate was not found");
                return response;
            }

            var symbol = await _symbolRepository.SelectByIdAsync(accountDerivate.SymbolId);
            if (symbol == null || symbol.Value != request.SymbolName)
            {
                response.SetMessage("Invalid symbol for account");
                return response;
            }
            request.Symbol = symbol;

            var trade = TradeMapper.ToTrade(request);
            var tradeOrders = request.OrderParameters!.Select(p => TradeMapper.ToOrderParameter(p, request.Symbol)).ToList();

            if (request.UseAccountQuantity)
            {
                var quantityRisked = decimal.Zero;
                var balanceRiskedAmount = account.GetBalanceRiskedAmount();
                var limitPrice = tradeOrders.First(order => order.OrderParameterType == OrderParameterType.Open).Price;
                if (!string.IsNullOrEmpty(request.OrderCloseParameterType))
                {
                    var closePrice = tradeOrders.First(order => order.OrderParameterType.ToString() == request.OrderCloseParameterType).Price;
                    quantityRisked = balanceRiskedAmount / Math.Abs(limitPrice - closePrice);
                }
                else if (request.UseDefaultTradeParameter || string.IsNullOrEmpty(request.CloseCondition))
                {
                    quantityRisked = balanceRiskedAmount / limitPrice;
                }
                else
                {
                    response.SetMessage("Unabled to set RiskedQuantity");
                }
                trade.SetTradeRiskedQuantity(quantityRisked);
            }

            if (request.UseDefaultTradeParameter)
            {
                var DefaultTradeParameter = await _defaultTradeParameter.SelectByIdAsync(account.Id);
                if (DefaultTradeParameter == null)
                {
                    response.SetMessage("Account does not have default conditionals");
                    return response;
                }
                trade.LoadValuesFromDefaultTradeParameter(DefaultTradeParameter);
            }

            trade.SetTrade(tradeOrders);

            response.MarkSuccess();
        }
        catch (Exception ex)
        {
            response.SetMessage(ex.Message);
        }
        return response;
    }

    private decimal GetDefaultAmountByTrade(decimal openPrice, decimal conditionalPrice, decimal riskedAmount, OrderSide side, decimal riskReward)
    {
        throw new NotImplementedException();
    }
}