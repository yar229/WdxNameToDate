using System;
using System.Text.RegularExpressions;

namespace WdxNameToDate
{
    class Program
    {
        static void Main(string[] args)
        {
            //string fileName = "zzz 2021-04-22 Thu test .zip";
            //string fileName = "zzz 22.04.2021 Thu test .zip";
            string fileName = "zzz 12-04-2021 Thu test .zip";

            (string expr, Func<Match, DateTime> eval)[] exprs =
            {
                (@"(?<=\A|\s)(?<d>\d{2})\.(?<m>\d{2})\.(?<y>\d{4})(?=\s|\Z|\.)", m => new DateTime(
                    int.Parse(m.Groups["y"].Value), 
                    int.Parse(m.Groups["m"].Value),
                    int.Parse(m.Groups["d"].Value))),

                (@"(?<=\A|\s)(?<d>\d{2})-(?<m>\d{2})-(?<y>\d{4})(?=\s|\Z|\.)", m => new DateTime(
                    int.Parse(m.Groups["y"].Value), 
                    int.Parse(m.Groups["m"].Value),
                    int.Parse(m.Groups["d"].Value))),

                (@"(?<=\A|\s)\d{4}-\d{2}-\d{2}\s+(sun|mon|tue|wed|thu|fri|sat)(?=\s|\Z|\.)", m => DateTime.Parse(m.Value)),
                (@"(?<=\A|\s)\d{4}-\d{2}-\d{2}(?=\s|\Z|\.)", m => DateTime.Parse(m.Value))
            };

            int count = 0;
            for (int i = 0 ; i < exprs.Length && 0 == count ; i++)
            {
                int ii = i;
                var rx = new Regex(exprs[i].expr, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                fileName = rx.Replace(fileName, match =>
                {
                    try
                    {
                        if (0 != count) 
                            return match.Value;

                        var date = exprs[ii].eval(match);
                        count++;
                        return date.ToString("yyyy-MM-dd ddd");
                    }
                    catch (Exception)
                    {
                        return match.Value;
                    }
                }, 1);
            }
        }
    }
}
