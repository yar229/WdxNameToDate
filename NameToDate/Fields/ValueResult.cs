using TcPluginBase.Content;

namespace YaR.TotalCommander.Wdx.NameToDate.Fields
{
    public class ValueResult
    {
        public GetValueResult Result { get; set; }
        public string Value { get;set; }

        public static ValueResult Success(string value)
        {
            return new() { Result = GetValueResult.Success, Value = value };
        }

        public static ValueResult OnDemand(string value)
        {
            return new() { Result = GetValueResult.OnDemand, Value = value };
        }

        public static ValueResult FieldEmpty()
        {
            return new() { Result = GetValueResult.OnDemand, Value = string.Empty };
        }

        public static ValueResult FileError()
        {
            return new() { Result = GetValueResult.FileError, Value = string.Empty };
        }
    }
}
