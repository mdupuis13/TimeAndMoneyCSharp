using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Info.MartinDupuis.DomainLanguage.Time
{
    /// <summary>
    /// 
    /// </summary>
    public class TimePoint : IComparable<TimePoint>
    {
        private static readonly TimeZoneInfo GMT = TimeZoneInfo.Utc;

        /// <summary>
        /// Epoch is frequently used to represent "the beginning of time".
        /// </summary>
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, CultureInfo.InstalledUICulture.Calendar);
    	long millisecondsFromEpoch;

        public static TimePoint AtMidnightGMT(int year, int month, int date)
        {
            return AtMidnight(year, month, date, GMT);
        }

        public static TimePoint AtMidnight(int year, int month, int date, TimeZoneInfo zone)
        {
            return At(year, month, date, 0, 0, 0, 0, zone);
        }

        public static TimePoint AtGMT(int year, int month, int date, int hour, int minute)
        {
            return AtGMT(year, month, date, hour, minute, 0);
        }

        public static TimePoint AtGMT(int year, int month, int date, int hour, int minute, int second)
        {
            return AtGMT(year, month, date, hour, minute, second, 0);
        }

        public static TimePoint AtGMT(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            return At(year, month, day, hour, minute, second, millisecond, GMT);
        }

        public static TimePoint At(int year, int month, int day, int hour, int minute, int second, TimeZoneInfo zone)
        {
            return At(year, month, day, hour, minute, second, 0, zone);
        }

        public static TimePoint At(int year, int month, int day, int hour, int minute, int second, int millisecond, TimeZoneInfo zone)
        {
            
            DateTime myDate = new DateTime(year, month, day, hour, minute, second, CultureInfo.InvariantCulture.Calendar);
            
            return From(myDate);
        }

        public static TimePoint At12hr(int year, int month, int date, int hour, string am_pm, int minute, int second, int millisecond, TimeZoneInfo zone)
        {
            return At(year, month, date, ConvertedTo24hour(hour, am_pm), minute, second, millisecond, zone);
        }

        private static int ConvertedTo24hour(int hour, String am_pm)
        {
            int translatedAmPm = am_pm.Equals("AM", StringComparison.InvariantCultureIgnoreCase) ? 0 : 12;
            translatedAmPm -= (hour == 12) ? 12 : 0;

            return hour + translatedAmPm;
        }

        public static TimePoint ParseGMTFrom(String dateString, String pattern)
        {
            return ParseFrom(dateString, pattern, GMT);
        }

        public static TimePoint ParseFrom(String dateString, String pattern, TimeZoneInfo zone)
        {
            DateTime date = DateTime.ParseExact(dateString, pattern, CultureInfo.InvariantCulture.DateTimeFormat);
            date.AddMilliseconds(zone.GetUtcOffset(DateTime.Now.ToUniversalTime()).Milliseconds);

            return From(date);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">A <seealso cref="DateTime"/>.</param>
        /// <returns></returns>
        public static TimePoint From(DateTime value)
        {
            return From(value.Subtract(Epoch).Milliseconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="milliseconds">Number of milliseconds since Epoch (1970-01-01 00:00:00)</param>
        /// <returns></returns>
        public static TimePoint From(long milliseconds)
        {
            TimePoint result = new TimePoint(milliseconds);

            return result;

        }

        private TimePoint(long milliseconds)
        {
            this.millisecondsFromEpoch = milliseconds;
        }


        public int CompareTo(TimePoint other)
        {
            throw new NotImplementedException();
        }

        // BEHAVIORAL METHODS
        public override bool Equals(Object other)
        {
            return
                ((TimePoint)other).millisecondsFromEpoch == this.millisecondsFromEpoch;
        }

        public override int GetHashCode()
        {
            return (int)millisecondsFromEpoch;
        }
    }
}
