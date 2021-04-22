using TcPluginBase.Content;

namespace YaR.TotalCommander.Wdx.NameToDate.Fields
{
    public abstract class TcField
    {
        public string Name { get; set; }
        public string Unit { get; set; } = "";
        public abstract ContentFieldType ContentType { get; }

        public abstract ValueResult GetValue(string fileName, GetValueFlags flags, ref bool getAborted);

        public DefaultSortOrder SortOrder { get; set; } = DefaultSortOrder.Asc;

        public SupportedFieldOptions Options { get; set; }
    }
}
