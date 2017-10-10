using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Info.MartinDupuis.DomainLanguage.Time
{
    /// <summary>
    /// 
    /// </summary>
    public class TimeUnit
    {
        public static readonly TimeUnit millisecond = new TimeUnit(Type.millisecond, Type.millisecond, 1);
        public static readonly TimeUnit second = new TimeUnit(Type.second, Type.millisecond, (int)TimeUnitConversionFactors.millisecondsPerSecond);
        public static readonly TimeUnit minute = new TimeUnit(Type.minute, Type.millisecond, (int)TimeUnitConversionFactors.millisecondsPerMinute);
        public static readonly TimeUnit hour = new TimeUnit(Type.hour, Type.millisecond, (int)TimeUnitConversionFactors.millisecondsPerHour);
        public static readonly TimeUnit day = new TimeUnit(Type.day, Type.millisecond, (int)TimeUnitConversionFactors.millisecondsPerDay);
        public static readonly TimeUnit week = new TimeUnit(Type.week, Type.millisecond, (int)TimeUnitConversionFactors.millisecondsPerWeek);
        public static readonly TimeUnit[] descendingMillisecondBased = { week, day, hour, minute, second, millisecond };
        public static readonly TimeUnit[] descendingMillisecondBasedForDisplay = { day, hour, minute, second, millisecond };
        public static readonly TimeUnit month = new TimeUnit(Type.month, Type.month, 1);
        public static readonly TimeUnit quarter = new TimeUnit(Type.quarter, Type.month, (int)TimeUnitConversionFactors.monthsPerQuarter);
        public static readonly TimeUnit year = new TimeUnit(Type.year, Type.month, (int)TimeUnitConversionFactors.monthsPerYear);
        public static readonly TimeUnit[] descendingMonthBased = { year, quarter, month };
        public static readonly TimeUnit[] descendingMonthBasedForDisplay = { year, month };
    
        private Type _type;
        private Type _baseType;
        private int _factor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="baseType"></param>
        /// <param name="factor"></param>
        protected TimeUnit(Type type, Type baseType, int factor)
        {
            this._type = type;
            this._baseType = baseType;
            this._factor = factor;
        }

        TimeUnit BaseUnit()
        {
            return _baseType.Equals(Type.millisecond) ? millisecond : month;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetFactor()
        {
            return _factor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsConvertibleToMilliseconds()
        {
            return IsConvertibleTo(millisecond);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsConvertibleTo(TimeUnit other)
        {
            return _baseType.Equals(other._baseType);
        }

        TimeUnit[] DescendingUnits()
        {
            return IsConvertibleToMilliseconds() ? descendingMillisecondBased : descendingMonthBased;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TimeUnit NextFinerUnit()
        {
            TimeUnit[] descending = DescendingUnits();
            int index = -1;

            for (int i = 0; i < descending.Length; i++)
            { 
                if (descending[i].Equals(this))
                    index = i;
            }

            if (index == descending.Length - 1)
                return null;

            return descending[index + 1];
        }

        /// <summary>
        /// Returns a value indicating whether this instance and a specified <seealso cref="Object"/> represent the same type and value.
        /// </summary>
        /// <param name="other">An object to compare to this instance.</param>
        /// <returns><code>true</code> if <code>other</code> is equal to this instance; otherwise, <code>false</code>.</returns>
        public override bool Equals(Object other)
        {
            if (other == null)
                return false;

            TimeUnit otherTU = (TimeUnit)other;

            return this._baseType.Equals(otherTU._baseType) 
                && this._factor == otherTU._factor 
                && this._type.Equals(otherTU._type);
        }

        /// <summary>
        /// Compares this instance to a specified <seealso cref="Object"/> and returns a comparison of their relative values.
        /// </summary>
        /// <param name="other">The object to compare with this instance, or <code>null</code>.</param>
        /// <returns>A signed number indicating the relative values of this instance and <code>other</code>.</returns>
        public int CompareTo(Object other)
        {
            TimeUnit otherTU = (TimeUnit)other;

            if (otherTU._baseType.Equals(_baseType))
                return _factor - otherTU._factor;

            if (_baseType.Equals(Type.month))
                return 1;

            return -1;
        }

        /// <summary>
        /// Returns a <seealso cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns>A <seealso cref="System.String"/> that represents this instance.</returns>
        public string ToString(long quantity)
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append(quantity);
            buffer.Append(" ");
            buffer.Append(_type.Name);
            buffer.Append(quantity == 1 ? "" : "s");

            return buffer.ToString();
        }

        /// <summary>
        /// Returns a <seealso cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <seealso cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return _type.Name;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return _factor + _baseType.GetHashCode() + _type.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        [Serializable()]
        protected class Type
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            public static readonly Type millisecond = new Type("millisecond");
            public static readonly Type second = new Type("second");
            public static readonly Type minute = new Type("minute");
            public static readonly Type hour = new Type("hour");
            public static readonly Type day = new Type("day");
            public static readonly Type week = new Type("week");
            public static readonly Type month = new Type("month");
            public static readonly Type quarter = new Type("quarter");
            public static readonly Type year = new Type("year");

            private string _name;

            public string Name 
            {
                get
                {
                    return _name;
                }
            }

            Type(string name)
            {
                this._name = name;
            }

            public override bool Equals(Object other)
            {
                try
                {
                    return Equals((Type)other);
                }
                catch (InvalidCastException)
                {
                    return false;
                }
            }

            public bool Equals(Type another)
            {
                return another != null 
                    && this._name.Equals(another._name);
            }

            public override int GetHashCode()
            {
                return _name.GetHashCode();
            }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        }

    }
}
