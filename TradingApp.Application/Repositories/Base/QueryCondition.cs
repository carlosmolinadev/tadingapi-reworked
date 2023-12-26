namespace TradingApp.Application.Repositories.Base
{
    public class QueryCondition
    {
        public QueryCondition(string column, string operatorSign, object value) { Column = column; Operator = operatorSign; Value = value; }
        public QueryCondition(string column, object value) : this(column, "=", value){}
        public string Column { get; set; }
        public string Operator { get; set; }
        public dynamic Value { get; set; }
    }
}