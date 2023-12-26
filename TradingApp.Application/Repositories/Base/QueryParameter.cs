namespace TradingApp.Application.Repositories.Base
{
    public class QueryParameter
    {
        public List<string> OrderByColumn { get; } = new();
        public List<QueryCondition> Conditions { get; } = new();
        public string Direction { get; private set; } = "asc";
        public int? Limit { get; private set; }
        public int? Offset { get; private set; }
        public static readonly string Eq = "=";
        public static readonly string He = ">";
        public static readonly string Ne = "<>";
        public static readonly string Lw = "<";

        public QueryParameter AddCondition(string column, string operatorSign, object value)
        {
            if (!ValidateOperator(operatorSign))
            {
                throw new Exception("Invalid Query Operator");
            }
            var queryCondition = new QueryCondition(column, operatorSign, value);
            Conditions.Add(queryCondition);
            return this;
        }

        public QueryParameter AddEqualCondition(string column, object value)
        {
            var queryCondition = new QueryCondition(column, value);
            Conditions.Add(queryCondition);
            return this;
        }

        public void AddOrderParameter(string column)
        {
            OrderByColumn.Add(column);
        }
        public void SetLimit(int limit)
        {
            Limit = limit;
        }
        public void SetOrderDesc()
        {
            Direction = "desc";
        }

        private static bool ValidateOperator(string queryOperator)
        {
            return queryOperator == QueryParameter.Eq && queryOperator == QueryParameter.He && queryOperator == QueryParameter.Ne && queryOperator == QueryParameter.Lw;
        }
    }
}