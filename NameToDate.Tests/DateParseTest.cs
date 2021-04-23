using Microsoft.VisualStudio.TestTools.UnitTesting;
using YaR.TotalCommander.Wdx.NameToDate.Fields;

namespace NameToDate.Tests
{
    [TestClass]
    public class DateParseTest
    {
        private readonly (string Input, string Output)[] _testDateStrings =
        {
            ("12-04-2021", "2021-04-12 Mon"),
            ("zzz 12-04-2021 test.zip", "zzz 2021-04-12 Mon test.zip"),
            ("zzz 12-04-2021 test.zip", "zzz 2021-04-12 Mon test.zip"),
            ("zzz 12-04-2021 test.zip", "zzz 2021-04-12 Mon test.zip"),

            ("zzz 22.04.2021 test.zip", "zzz 2021-04-22 Thu test.zip"),

            ("Monday, June 15, 2009", "2009-06-15 Mon"),
            ("Mon, Jun 15, 2009", "2009-06-15 Mon"),
            ("Jun 15, 2009", "2009-06-15 Mon")
        };

        [TestMethod]
        public void TestInputDateFormats()
        {
            var fld = new TcFieldFilenameAsDate("yyyy-MM-dd ddd");

            foreach (var pair in _testDateStrings)
                Assert.AreEqual(pair.Output, fld.GetValue(pair.Input));
        }
    }
}
