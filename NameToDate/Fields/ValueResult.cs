using TcPluginBase.Content;

namespace YaR.TotalCommander.Wdx.NameToDate.Fields
{
    class ValueResult
    {
        public GetValueResult Result { get; set; }
        public string Value { get;set; }

        public static ValueResult Success(string value)
        {
            return new ValueResult { Result = GetValueResult.Success, Value = value };
        }

        public static ValueResult OnDemand(string value)
        {
            return new ValueResult { Result = GetValueResult.OnDemand, Value = value };
        }

        public static ValueResult FieldEmpty()
        {
            return new ValueResult { Result = GetValueResult.OnDemand, Value = string.Empty };
        }

        public static ValueResult FileError()
        {
            return new ValueResult { Result = GetValueResult.FileError, Value = string.Empty };
        }
    }
}
