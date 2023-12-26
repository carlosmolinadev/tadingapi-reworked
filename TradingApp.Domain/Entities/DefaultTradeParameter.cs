using TradingApp.Domain.Common;
using TradingApp.Domain.Enums;

namespace TradingApp.Domain.Entities
{
    public class DefaultTradeParameter : Entity<Guid>
    {
        public DefaultTradeParameter(decimal riskReward, bool candleClosedEntry, int delayedEntry, decimal closeValue, string closeCondition, Guid accountId, int retryAttempt) : base(accountId)
        {
            SetCloseCondition(closeCondition, closeValue);
            RiskReward = riskReward;
            CandleClosedEntry = candleClosedEntry;
            DelayedEntry = delayedEntry;
            AccountId = accountId;
            RetryAttempt = retryAttempt;
        }

        public decimal RiskReward { get; set; }
        public bool CandleClosedEntry { get; set; }
        public int DelayedEntry { get; set; }
        public decimal CloseValue { get; set; }
        public string CloseCondition { get; set; }
        public int RetryAttempt {get; set;}
        public Guid AccountId { get; set; }

        public void SetCloseCondition(string condition, decimal conditionValue)
        {
            if (!Enum.TryParse(condition, true, out CloseConditionOption enumCondition))
            {
                throw new Exception("Condition is not valid");
            }
            CloseCondition = condition;
            if (enumCondition == CloseConditionOption.CandleWick)
            {
                CloseValue = decimal.Zero;
            }
            else
            {
                CloseValue = conditionValue;
            }
        }
    }
}
