using System;
using System.Text.RegularExpressions;
using YaR.TotalCommander.Wdx.NameToDate.Fields;

namespace WdxNameToDate
{
    class Program
    {
        static void Main(string[] args)
        {

            var fld = new TcFieldFilenameAsDate("yyyy-MM-dd ddd");

            string[] strs =
            {
                "12-04-2021",
                "zzz 12-04-2021 test.zip",
                "zzz 12-04-2021 Thu test.zip",
                "zzz 22.04.2021 Thu test.zip",
                "zzz 12-04-2021 Thu test.zip",
                "Monday, June 15, 2009"
            };

            foreach (string str in strs)
                Console.WriteLine($"{str}\t\t{fld.Test(str)}");
        }
    }
}
