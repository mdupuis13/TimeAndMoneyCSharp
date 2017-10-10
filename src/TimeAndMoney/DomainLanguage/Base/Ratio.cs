using System;

namespace Info.MartinDupuis.DomainLanguage.Base
{
    /// <summary>
    /// Ratio represents the unitless division of two quantities of the same type.
    /// <para>The key to its usefulness is that it defers the calculation of a decimal
    /// value for the ratio. An object which has responsibility for the two values in
    /// the ratio and understands their quantities can create the ratio, which can
    /// then be used by any client in a unitless form, so that the client is not
    /// required to understand the units of the quantity.At the same time, this
    /// gives control of the precision and rounding rules to the client, when the
    /// time comes to compute a decimal value for the ratio. The client typically has
    /// the responsibilities that enable an appropriate choice of these parameters.</para>
    /// </summary>
    public class Ratio : IEquatable<Ratio>
    {
        private decimal numerator;
        private decimal denominator;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        #region Ctors
        public Ratio(decimal numerator, decimal denominator)
        {
            this.numerator = numerator;
            this.denominator = denominator;
        }

        public static Ratio Of(long numerator, long denominator)
        {
            return new Ratio(numerator, denominator);
        }

        public static Ratio Of(decimal numerator, decimal denominator)
        {
            return new Ratio(numerator, denominator);
        }

        public static Ratio Of(decimal fractional)
        {
            return new Ratio(fractional, new decimal(1));
        }
        #endregion

        public decimal DecimalValue(int scale, MidpointRounding roundingRule)
        {
            decimal divResult = decimal.Divide(numerator, denominator);

            return decimal.Round(divResult, scale, roundingRule);
        }

        public Ratio Times(decimal multiplier)
        {
            return Ratio.Of(decimal.Multiply(numerator, multiplier), denominator);
        }

        public Ratio Times(Ratio multiplier)
        {
            return Ratio.Of(decimal.Multiply(numerator, multiplier.numerator), decimal.Multiply(denominator, multiplier.denominator));
        }

        #region IEquatable overloads
        public override bool Equals(Object other)
        {
            try
            {
                return Equals((Ratio)other);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Equals(Ratio other)
        {
            return
                other != null
                && this.numerator.Equals(other.numerator)
                && this.denominator.Equals(other.denominator);
        }
        #endregion

        #region Default overrides
        public override int GetHashCode()
        {
            return numerator.GetHashCode();
        }

        public override String ToString()
        {
            return numerator.ToString() + "/" + denominator.ToString();
        }
        #endregion
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    }
}
