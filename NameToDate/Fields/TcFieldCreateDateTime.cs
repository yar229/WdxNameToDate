using System;
using System.IO;
using TcPluginBase.Content;

namespace YaR.TotalCommander.Wdx.NameToDate.Fields
{


    internal class TcFieldCreateDateTime : TcField
    {
        public TcFieldCreateDateTime(ContentFieldType mode)
        {
            _mode = mode;

            var opt = mode switch
            {
                ContentFieldType.Date => SupportedFieldOptions.SubstDate,
                ContentFieldType.Time => SupportedFieldOptions.SubstTime,
                ContentFieldType.DateTime => SupportedFieldOptions.SubstDateTime,
                _ => throw new ArgumentException("invalid enum value", paramName: nameof(_mode))
            };
            Options = SupportedFieldOptions.Edit | opt;
        }

        private readonly ContentFieldType _mode;

        public override ContentFieldType ContentType => _mode;

        public override ValueResult GetValue(string fileName, GetValueFlags flags, ref bool getAborted)
        {
            if (Directory.Exists(fileName))
            {
                var info = new DirectoryInfo(fileName);
                string timeString = info.CreationTime.ToString("G");
                return ValueResult.Success(timeString);
            }
            
            if (File.Exists(fileName))
            {
                var info = new FileInfo(fileName);
                string timeString = info.CreationTime.ToString("G");
                return ValueResult.Success(timeString);
            }

            return ValueResult.FileError();
        }
    }
}
