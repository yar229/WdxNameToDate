using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using YaR.TotalCommander.Wdx.NameToDate.Fields;

namespace NameToDate.Tests
{
    [TestClass]
    public class DateParseTest
    {
        private readonly (string Input, CultureInfo CultureInfo, string Output)[] _testDateStrings =
        {
            ("12-04-2021",              _ciEng, "2021-04-12 Mon"),
            ("12-04-2021",              _ciRus, "2021-04-12 Ïí"),

            ("zzz 12-04-2021 test.zip", _ciEng, "zzz 2021-04-12 Mon test.zip"),
            ("zzz 12-04-2021 test.zip", _ciEng, "zzz 2021-04-12 Mon test.zip"),
            ("zzz 12-04-2021 test.zip", _ciEng, "zzz 2021-04-12 Mon test.zip"),

            ("zzz 22.04.2021 test.zip", _ciEng, "zzz 2021-04-22 Thu test.zip"),

            ("Monday, June 15, 2009",   _ciEng, "2009-06-15 Mon"),
            ("Mon, Jun 15, 2009",       _ciEng, "2009-06-15 Mon"),
            ("Jun 15, 2009",            _ciEng, "2009-06-15 Mon")
        };

        private static CultureInfo _ciEng = CultureInfo.GetCultureInfo("en-GB");
        private static CultureInfo _ciRus = CultureInfo.GetCultureInfo("ru-RU");

        [TestMethod]
        public void TestInputDateFormats()
        {
            foreach (var testData in _testDateStrings)
            {
                var fld = new TcFieldFilenameAsDate("yyyy-MM-dd ddd", testData.CultureInfo);
                Assert.AreEqual(testData.Output, fld.GetValue(testData.Input), ignoreCase: true);
            }
        }
    }
}
