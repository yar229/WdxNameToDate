using System;
using System.IO;
using System.Text.RegularExpressions;
using TcPluginBase.Content;

namespace YaR.TotalCommander.Wdx.NameToDate.Fields
{
    class TcFieldFilenameAsDate : TcField
    {
        public TcFieldFilenameAsDate(string dateFormat)
        {
            _dateFormat = dateFormat;
        }

        private readonly string _dateFormat;

        public override ContentFieldType ContentType => ContentFieldType.String;

        public override ValueResult GetValue(string fileName, GetValueFlags flags, ref bool getAborted)
        {
            var info = new FileInfo(fileName);
            return ValueResult.Success(ReplaceDates(info.Name));
        }

        private readonly (string expr, Func<Match, DateTime> eval)[] _dateExpressions =
        {
            (@"(?<=\A|\s)(?<d>\d{2})\.(?<m>\d{2})\.(?<y>\d{4})(?=\s|\Z|\.)", m => m.Parse("y", "m", "d")),

            (@"(?<=\A|\s)(?<d>\d{2})-(?<m>\d{2})-(?<y>\d{4})(?=\s|\Z|\.)", m => m.Parse("y", "m", "d")),

            (@"(?<=\A|\s)\d{4}-\d{2}-\d{2}\s+(sun|mon|tue|wed|thu|fri|sat)(?=\s|\Z|\.)", m => DateTime.Parse(m.Value)),
            (@"(?<=\A|\s)\d{4}-\d{2}-\d{2}(?=\s|\Z|\.)", m => DateTime.Parse(m.Value))
        };

        private string ReplaceDates(string data)
        {
            string res = data;

            int count = 0;
            for (int i = 0 ; i < _dateExpressions.Length && 0 == count ; i++)
            {
                int ii = i;
                var rx = new Regex(_dateExpressions[i].expr, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                res = rx.Replace(res, match =>
                {
                    try
                    {
                        if (0 != count) 
                            return match.Value;

                        var date = _dateExpressions[ii].eval(match);
                        count++;
                        return date.ToString(_dateFormat);
                    }
                    catch (Exception)
                    {
                        return match.Value;
                    }
                }, 1);
            }

            return res;
        }
    }
}
