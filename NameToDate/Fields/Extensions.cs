using System;
using System.Text.RegularExpressions;

namespace YaR.TotalCommander.Wdx.NameToDate.Fields
{
    internal static class Extensions
    {
        public static DateTime Parse(this Match m, string yearGroupName, string monthGroupName, string dayGroupName)
        {
            return new (
                int.Parse(m.Groups[yearGroupName].Value),
                int.Parse(m.Groups[monthGroupName].Value),
                int.Parse(m.Groups[dayGroupName].Value));
        }

    }
}