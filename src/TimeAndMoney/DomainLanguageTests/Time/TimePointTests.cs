using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Info.MartinDupuis.DomainLanguage.Time.Tests
{
    public class TimePointTests
    {
        private static readonly string AM = "AM";
        private static readonly string PM = "PM";

        private TimeZoneInfo gmt = TimeZoneInfo.Utc;
        private TimeZoneInfo pt = TimeZoneInfo.GetSystemTimeZones()
                                    .Where(tz => tz.StandardName == "America/Los_Angeles")
                                    .FirstOrDefault();
        private TimeZoneInfo ct = TimeZoneInfo.GetSystemTimeZones()
                                    .Where(tz => tz.StandardName == "America/Chicago")
                                    .FirstOrDefault();
        private TimePoint dec19_2003 = TimePoint.AtMidnightGMT(2003, 12, 19);
        private TimePoint dec20_2003 = TimePoint.AtMidnightGMT(2003, 12, 20);
        private TimePoint dec21_2003 = TimePoint.AtMidnightGMT(2003, 12, 21);
        private TimePoint dec22_2003 = TimePoint.AtMidnightGMT(2003, 12, 22);

        [Fact]
        public void TestCreationWithDefaultTimeZone()
        {
            TimePoint expected = TimePoint.AtGMT(2004, 1, 1, 0, 0, 0, 0);

            Assert.Equal(expected, TimePoint.AtMidnightGMT(2004, 1, 1));    // at midnight
            Assert.Equal(expected, TimePoint.AtGMT(2004, 1, 1, 0, 0));      // hours in 24hr clock
            Assert.Equal(expected, TimePoint.At12hr(2004, 1, 1, 12, AM, 0, 0, 0, gmt)); // hours in 12hr clock

            Assert.Equal(expected, TimePoint.ParseGMTFrom("2004/01/01", "yyyy/MM/dd"));   // date from formatted String

            Assert.Equal(TimePoint.AtGMT(2004, 1, 1, 12, 0), 
                         TimePoint.At12hr(2004, 1, 1, 12, PM, 0, 0, 0, gmt));   // pm hours in 12hr clock
        }

    }
}
