using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xunit;

namespace Info.MartinDupuis.DomainLanguage.Time.Tests
{
    public class TimeUnitTests
    {
        [Fact]
        public void testToString()
        {
            Assert.Equal("month", TimeUnit.month.ToString());
        }

        [Fact]
        public void TestConvertibleToMilliseconds()
        {
            Assert.True(TimeUnit.millisecond.IsConvertibleToMilliseconds());
            Assert.True(TimeUnit.hour.IsConvertibleToMilliseconds());
            Assert.True(TimeUnit.day.IsConvertibleToMilliseconds());
            Assert.True(TimeUnit.week.IsConvertibleToMilliseconds());
            Assert.False(TimeUnit.month.IsConvertibleToMilliseconds());
            Assert.False(TimeUnit.year.IsConvertibleToMilliseconds());
        }

        [Fact]
        public void TestComparison()
        {
            Assert.Equal(0, TimeUnit.hour.CompareTo(TimeUnit.hour));
            Assert.True(TimeUnit.hour.CompareTo(TimeUnit.millisecond) > 0);
            Assert.True(TimeUnit.millisecond.CompareTo(TimeUnit.hour) < 0);
            Assert.True(TimeUnit.day.CompareTo(TimeUnit.hour) > 0);
            Assert.True(TimeUnit.hour.CompareTo(TimeUnit.day) < 0);

            Assert.True(TimeUnit.month.CompareTo(TimeUnit.day) > 0);
            Assert.True(TimeUnit.day.CompareTo(TimeUnit.month) < 0);
            Assert.True(TimeUnit.quarter.CompareTo(TimeUnit.hour) > 0);

            Assert.Equal(0, TimeUnit.month.CompareTo(TimeUnit.month));
            Assert.True(TimeUnit.quarter.CompareTo(TimeUnit.year) < 0);
            Assert.True(TimeUnit.year.CompareTo(TimeUnit.quarter) > 0);
        }

        [Fact]
        public void TestIsConvertableTo()
        {
            Assert.True(TimeUnit.hour.IsConvertibleTo(TimeUnit.minute));
            Assert.True(TimeUnit.minute.IsConvertibleTo(TimeUnit.hour));    
            Assert.True(TimeUnit.year.IsConvertibleTo(TimeUnit.month));
            Assert.True(TimeUnit.month.IsConvertibleTo(TimeUnit.year));
            Assert.False(TimeUnit.month.IsConvertibleTo(TimeUnit.hour));
            Assert.False(TimeUnit.hour.IsConvertibleTo(TimeUnit.month));
        }

        [Fact]
        public void TestNextFinerUnit()
        {
            Assert.Equal(TimeUnit.minute, TimeUnit.hour.NextFinerUnit());
            Assert.Equal(TimeUnit.month, TimeUnit.quarter.NextFinerUnit());
        }

    }
}
