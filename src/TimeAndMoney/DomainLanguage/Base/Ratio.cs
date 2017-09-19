using System;

namespace DomainLanguage.Base
{
    public class Ratio : IEquatable<Ratio>
    {
        private decimal numerator;
        private decimal denominator;

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

        public Ratio(decimal numerator, decimal denominator)
        {
            this.numerator = numerator;
            this.denominator = denominator;
        }

        public decimal DecimalValue(int scale, MidpointRounding roundingRule)
        {
            decimal divResult = decimal.Divide(numerator, denominator);

            return decimal.Round(divResult, scale, roundingRule);
        }

        public override bool Equals(Object anObject)
        {
            try
            {
                return Equals((Ratio)anObject);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Equals(Ratio other)
        {
            return
                other != null &&
                this.numerator.Equals(other.numerator) && this.denominator.Equals(other.denominator);
        }

        public Ratio Times(decimal multiplier)
        {
            return Ratio.Of(decimal.Multiply(numerator, multiplier), denominator);
        }

        public Ratio Times(Ratio multiplier)
        {
            return Ratio.Of(decimal.Multiply(numerator, multiplier.numerator), decimal.Multiply(denominator, multiplier.denominator));
        }

        public override int GetHashCode()
        {
            return numerator.GetHashCode();
        }

        public override String ToString()
        {
            return numerator.ToString() + "/" + denominator.ToString();
        }

    }
}
