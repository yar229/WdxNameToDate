using System;
using System.IO;
using System.Text.RegularExpressions;
using TcPluginBase.Content;

namespace YaR.TotalCommander.Wdx.NameToDate.Fields
{
    public class TcFieldFilenameAsDate : TcField
    {
        public TcFieldFilenameAsDate(string dateFormat)
        {
            _dateFormat = dateFormat;
        }

        private readonly string _dateFormat;

        public override ContentFieldType ContentType => ContentFieldType.String;

        public string Test(string fileName)
        {
            bool getaborted = false;

            return GetValue(fileName, GetValueFlags.None, ref getaborted).Value;
        }

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
            (@"(?<=\A|\s)\d{4}-\d{2}-\d{2}(?=\s|\Z|\.)", m => DateTime.Parse(m.Value)),

            //Monday, June 15, 2009
            (@"(?snx-)(?<=\A|\s)(Monday|Mon|Tuesday|Tue|Wednesday|Wed|Thursday|Thu|Friday|Fri|Saturday|Sat|Sunday|Sun)(,\s*)? (January|Jan|March|Mar|May|July|Jul|June|Jun|August|Aug|Sep|SeptemberOctober|Oct|December|Dec) \s* \d{2} ,? \s* \d{4}(?=\s|\Z|\.)", m => DateTime.Parse(m.Value)),

            (@"\b(?:(?:31(\/|-| |\.)(?:0?[13578]|1[02]|(?:Jan|January|Mar|March|May|Jul|July|Aug|August|Oct|October|Dec|December)))\1|(?:(?:29|30)(\/|-| |\.)(?:0?[1,3-9]|1[0-2]|(?:Jan|January|Mar|March|Apr|April|May|Jun|June|Jul|July|Aug|August|Sep|September|Oct|October|Nov|November|Dec|December))\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})\b|\b(?:29(\/|-| |\.)(?:0?2|(?:Feb|February))\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))\b|\b(?:0?[1-9]|1\d|2[0-8])(\/|-| |\.)(?:(?:0?[1-9]|(?:Jan|January|Feb|February|Mar|March|Apr|April|May|Jun|June|Jul|July|Aug|August|Sep|September))|(?:1[0-2]|(?:Oct|October|Nov|November|Dec|December)))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})\b",
                m => DateTime.Parse(m.Value))
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
