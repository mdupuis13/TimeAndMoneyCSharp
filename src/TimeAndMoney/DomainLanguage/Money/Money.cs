/*
 * Copyright (c) 2004 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using CSharp.Util.Currency;
using Info.MartinDupuis.DomainLanguage.Base;

namespace Info.MartinDupuis.DomainLanguage.Money
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable()]
    public class Money : IEquatable<Money>, IComparable<Money>
    {
        [NonSerialized()] private static readonly Currency CAD = Currency.GetByLetterCode("CAD");
        [NonSerialized()] private static readonly Currency EUR = Currency.GetByLetterCode("EUR");
        [NonSerialized()] private static readonly Currency USD = Currency.GetByLetterCode("USD");
        [NonSerialized()] private static readonly MidpointRounding DEFAULT_ROUNDING_MODE = MidpointRounding.ToEven;

        private decimal _amount;
        private Currency _currency;

        #region Ctors
        /// <summary>
        /// The constructor does not complex computations and requires simple 
        /// inputs consistent with the class invariant. 
        /// 
        /// It assumes the currency of the CurrentCulture.
        /// 
        /// Other creation methods are available for convenience.
        /// </summary>
        /// <param name="amount"></param>
        public Money(decimal amount)
        {
            var currentRegion = new RegionInfo(CultureInfo.CurrentCulture.LCID);

            this._currency = Currency.GetByLetterCode(currentRegion.ISOCurrencySymbol);
            this._amount = decimal.Round(amount, _currency.GetDefaultFractionDigits(), DEFAULT_ROUNDING_MODE);
        }

        /// <summary>
        /// The constructor does not complex computations and requires simple 
        /// inputs consistent with the class invariant. 
        /// Other creation methods are available for convenience.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        public Money(decimal amount, Currency currency)
        {
            this._currency = currency;
            this._amount = decimal.Round(amount, currency.GetDefaultFractionDigits(), DEFAULT_ROUNDING_MODE);
        }
        #endregion

        #region Publics
        #region Public Static
        /// <summary>
        /// This creation method is safe to use. It will adjust scale, but will not round off the amount.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public static Money ValueOf(decimal amount, Currency currency)
        {
            return Money.ValueOf(amount, currency, DEFAULT_ROUNDING_MODE);
        }

        /// <summary>
        /// For convenience, an amount can be rounded to create a Money. 
        /// </summary>
        /// <param name="rawAmount"></param>
        /// <param name="currency"></param>
        /// <param name="roundingMode"></param>
        /// <returns></returns>
        public static Money ValueOf(decimal rawAmount, Currency currency, MidpointRounding roundingMode)
        {
            decimal amount = decimal.Round(rawAmount, currency.GetDefaultFractionDigits(), roundingMode);
            return new Money(amount, currency);
        }

        /// <summary>
        /// WARNING: Because of the indefinite precision of double, this method must round off the value.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public static Money ValueOf(double amount, Currency currency)
        {
            return Money.ValueOf(amount, currency, DEFAULT_ROUNDING_MODE);
        }

        /// <summary>
        /// Because of the indefinite precision of double, this method must round off the value. This method gives the client control of the rounding mode.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <param name="roundingMode"></param>
        /// <returns></returns>
        public static Money ValueOf(double amount, Currency currency, MidpointRounding roundingMode)
        {
            decimal rawAmount = new decimal(amount);
            return Money.ValueOf(rawAmount, currency, roundingMode);
            
        }

        #region Factory methods for oft-used currencies
        /// <summary>
        /// WARNING: Because of the indefinite precision of double, thismethod must round off the value.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static Money USDollars(double amount)
        {
            return Money.ValueOf(amount, USD);
        }

        /// <summary>
        /// This creation method is safe to use. It will adjust scale, but will not round off the amount.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static Money USDollars(decimal amount)
        {
            return Money.ValueOf(amount, USD);
        }

        /// <summary>
        /// WARNING: Because of the indefinite precision of double, thismethod must round off the value.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static Money CADollars(double amount)
        {
            return Money.ValueOf(amount, CAD);
        }

        /// <summary>
        /// This creation method is safe to use. It will adjust scale, but will not round off the amount.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static Money CADollars(decimal amount)
        {
            return Money.ValueOf(amount, CAD);
        }

        /// <summary>
        /// WARNING: Because of the indefinite precision of double, thismethod must round off the value.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static Money Euros(double amount)
        {
            return Money.ValueOf(amount, EUR);
        }

        /// <summary>
        /// This creation method is safe to use. It will adjust scale, but will not round off the amount.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static Money Euros(decimal amount)
        {
            return Money.ValueOf(amount, EUR);
        }

        #endregion

        /// <summary>
        /// Adds all items in the collection received.
        /// </summary>
        /// <param name="monies">Collection of <code>Money</code> objects</param>
        /// <returns><code>Money</code> object that is the sum of all items in <code>monies</code>.</returns>
        public static Money Sum(ICollection<Money> monies)
        {            
            if (monies.Count == 0)
                return new Money(0);

            Money sum = new Money(0);

            foreach (Money each in monies)
            {
                sum = sum.Plus(each);
            }

            return sum;
        }

        #endregion

        /*
         * How best to handle access to the internals? It is needed for
         * database mapping, UI presentation, and perhaps a few other
         * uses. Yet giving public access invites people to do the
         * real work of the Money object elsewhere.
         * Here is an experimental approach, giving access with a 
         * warning label of sorts. Let us know how you like it.
         */

        /// <summary>
        /// Returns the RAW value of this Money object.
        /// <para>This breaks encapsulation, but may be neeed for some 
        /// functionnality yet unforeseen.
        /// </para>
        /// </summary>
        public decimal BreachEncapsulationOfAmount
        {
            get { return _amount; }
            set { _amount = value; }    
        }

        /// <summary>
        /// Returns the RAW currency of this Money object.
        /// <para>This breaks encapsulation, but may be neeed for some 
        /// functionnality yet unforeseen.
        /// </para>
        /// </summary>
        public Currency BreachEncapsulationOfCurrency
        {
            get { return _currency; }
            set { _currency = value; }
        }

        /// <summary>
        /// Negates the value of this <code>Money's</code> value.
        /// </summary>
        /// <returns>A new <code>Money</code> object with this <code>Money's</code> value negated.</returns>
        public Money Negated()
        {
            return Money.ValueOf(decimal.Negate(_amount), _currency);
        }

        /// <summary>
        /// Multiply this object's value with a decimal applying default rounding behavior.
        /// </summary>
        /// <param name="factor"><seealso cref="decimal"/>decimal value to multiply with.</param>
        /// <returns>New Money object.</returns>
        public Money Times(decimal factor)
        {
            return Times(factor, DEFAULT_ROUNDING_MODE);
        }


        /*
         * TODO: decimal.multiply() scale is sum of scales of two multiplied numbers. So what is scale of times?
         */

        /// <summary>
        /// Multiply this object's value with a decimal applying specified rounding behavior.
        /// </summary>
        /// <param name="factor"><seealso cref="decimal"/>decimal value to multiply with.</param>
        /// <param name="roundingMode">MidpointRounding mode to apply after the operation.</param>
        /// <returns>New Money object.</returns>
        public Money Times(decimal factor, MidpointRounding roundingMode)
        {
            return Money.ValueOf(decimal.Multiply(_amount, factor), _currency, roundingMode);
        }

        /// <summary>
        /// Multiply this object's value with a decimal applying the specified rounding behavior.
        /// </summary>
        /// <param name="amount"><seealso cref="double"/> value to multiply with.</param>
        /// <param name="roundingMode">MidpointRounding mode to apply after the operation.</param>
        /// <returns>New Money object.</returns>
        public Money Times(double amount, MidpointRounding roundingMode)
        {
            return Times(new decimal(amount), roundingMode);
        }

        /// <summary>
        /// Multiply this object's value with a decimal applying default rounding behavior.
        /// </summary>
        /// <param name="amount"><seealso cref="double"/> value to multiply with.</param>
        /// <returns>New Money object.</returns>
        public Money Times(double amount)
        {
            return Times(new decimal(amount));
        }

        /// <summary>
        /// Multiply this object's value with an integer applying default rounding behavior.
        /// </summary>
        /// <param name="factor"><seealso cref="int"/> value to multiply with.</param>
        /// <returns>New Money object.</returns>
        public Money Times(int factor)
        {
            return Times(new decimal(factor));
        }

        /*
         * TODO: Many apps require carrying extra precision in intermediate
         * calculations. The use of Ratio is a beginning, but need a comprehensive
         * solution. Currently, an invariant of Money is that the scale is the
         * currencies standard scale, but this will probably have to be suspended or
         * elaborated in intermediate calcs, or handled with defered calculations
         * like Ratio.
         */

        /// <summary>
        /// This probably should be Currency responsibility. Even then, it may need to be customized for specialty apps 
        /// because there are other cases, where the smallest increment is not the smallest unit.
        /// </summary>
        /// <returns></returns>
        public Money MinimumIncrement()
        {
            decimal one = new decimal(1);
            decimal increment = decimal.Multiply(one, (decimal)System.Math.Pow(10, -_currency.GetDefaultFractionDigits()));
            return Money.ValueOf(increment, _currency);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Money Incremented()
        {
            return this.Plus(MinimumIncrement());
        }

        /// <summary>
        /// Absolute value
        /// </summary>
        /// <returns>Absolute value of this object's instance.</returns>
        public Money Abs()
        {
            return Money.ValueOf(System.Math.Abs(_amount), _currency);
        }

        /// <summary>
        /// Adds the provided <code>Money</code> object to this one.
        /// </summary>
        /// <param name="other"><code>Money</code> object to add.</param>
        /// <returns>New <code>Money</code> object.</returns>
        public Money Plus(Money other)
        {
            AssertHasSameCurrencyAs(other);
            return Money.ValueOf(decimal.Add(_amount, other._amount), _currency);
        }


        /// <summary>
        /// Subtracts the provided <code>Money</code> object from this one.
        /// </summary>
        /// <param name="other"><code>Money</code> object to subtract.</param>
        /// <returns>New <code>Money</code> object.</returns>
        public Money Minus(Money other)
        {
            return this.Plus(other.Negated());
        }

        /// <summary>
        /// Divides the provided <code>Money</code> object to this one using the default MidpointRounding method.
        /// </summary>
        /// <param name="divisor"><code>Money</code> object to divide with.</param>
        /// <returns>New <code>Money</code> object.</returns>
        public Money DividedBy(double divisor)
        {
            return DividedBy(divisor, DEFAULT_ROUNDING_MODE);
        }

        /// <summary>
        /// Divides the provided <seealso cref="double"/> value to this one using the provided MidpointRounding method.
        /// </summary>
        /// <param name="divisor"><seealso cref="double"/> value to divide with.</param>
        /// <param name="roundingMode"><seealso cref="MidpointRounding"/> to use.</param>
        /// <returns>New <code>Money</code> object.</returns>
        public Money DividedBy(double divisor, MidpointRounding roundingMode)
        {
            return DividedBy(new decimal(divisor), roundingMode);
        }

        /// <summary>
        /// Divides the provided <seealso cref="decimal"/> value to this one using the provided MidpointRounding method.
        /// </summary>
        /// <param name="divisor"><seealso cref="decimal"/> value to divide with.</param>
        /// <param name="roundingMode"><seealso cref="MidpointRounding"/> to use.</param>
        /// <returns>New <code>Money</code> object.</returns>
        public Money DividedBy(decimal divisor, MidpointRounding roundingMode)
        {
            decimal newAmount = Decimal.Divide(_amount, divisor);
            return Money.ValueOf(newAmount, _currency, roundingMode);
        }

        /// <summary>
        /// Divides the provided <code>Money</code> object to this one returning the ratio of them.
        /// </summary>
        /// <param name="divisor"><code>Money</code> object to divide with.</param>
        /// <returns><see cref="Ratio"/> object.</returns>
        public Ratio DividedBy(Money divisor)
        {
            AssertHasSameCurrencyAs(divisor);
            return Ratio.Of(_amount, divisor._amount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ratio"></param>
        /// <param name="roundingRule"></param>
        /// <returns></returns>
        public Money Applying(Ratio ratio, MidpointRounding roundingRule)
        {
            return Applying(ratio, _currency.GetDefaultFractionDigits(), roundingRule);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ratio"></param>
        /// <param name="scale"></param>
        /// <param name="roundingRule"></param>
        /// <returns></returns>
        public Money Applying(Ratio ratio, int scale, MidpointRounding roundingRule)
        {
            decimal newAmount = ratio.Times(_amount).DecimalValue(scale, roundingRule);
            return Money.ValueOf(newAmount, _currency);
        }

        /// <summary>
        /// Returns the RAW value of this Money object.
        /// </summary>
        /// <returns></returns>
        public decimal GetAmount()
        {
            return _amount;
        }

        /// <summary>
        /// Returns the RAW currency of this Money object.
        /// </summary>
        /// <returns></returns>
        public Currency GetCurrency()
        {
            return _currency;
        }

        /// <summary>
        /// Indicates if this <code>Money</code> object is negative or not.
        /// </summary>
        /// <returns><code>true</code> if the amount is &lt; 0, otherwise, <code>false</code>.</returns>
        public bool IsNegative()
        {
            return _amount.CompareTo(new decimal(0)) < 0;
        }

        /// <summary>
        /// Indicates if this <code>Money</code> object is positive or not.
        /// </summary>
        /// <returns><code>true</code> if the amount is &gt; or equal to 0, otherwise, <code>false</code>.</returns>
        public bool IsPositive()
        {
            return _amount.CompareTo(new decimal(0)) >= 0;
        }

        /// <summary>
        /// Indicates if this <code>Money</code> object is equal to zero value or not.
        /// </summary>
        /// <returns><code>true</code> if the amount is equal to 0, otherwise, <code>false</code>.</returns>
        public bool IsZero()
        {
            return this.Equals(Money.ValueOf(0.0, _currency));
        }

        /// <summary>
        /// Indicates if this <code>Money</code> object is greater than the <code>other</code> value.
        /// </summary>
        /// <param name="other">The <code>Money</code> object to compare with this instance, or <code>null</code>.</param>
        /// <returns><code>true</code> if the object is &gt; <code>other</code>, otherwise, <code>false</code>.</returns>
        public bool IsGreaterThan(Money other)
        {
            return (CompareTo(other) > 0);
        }

        /// <summary>
        /// Indicates if this <code>Money</code> object is less than the <code>other</code> value.
        /// </summary>
        /// <param name="other">The <code>Money</code> object to compare with this instance, or <code>null</code>.</param>
        /// <returns><code>true</code> if the object is &lt; <code>other</code>, otherwise, <code>false</code>.</returns>
        public bool IsLessThan(Money other)
        {
            return (CompareTo(other) < 0);
        }

        // IComparable implementation
        /// <summary>
        /// Compares this instance to a specified <seealso cref="Object"/> and returns a comparison of their relative values.
        /// </summary>
        /// <param name="other">The object to compare with this instance, or <code>null</code>.</param>
        /// <returns>A signed number indicating the relative values of this instance and <code>other</code>.</returns>
        public int CompareTo(Object other)
        {
            return CompareTo((Money)other);
        }

        /// <summary>
        /// Compares this instance to a specified <seealso cref="Decimal"/> object and returns a comparison of their relative values.
        /// </summary>
        /// <param name="other">The <code>Money</code> object to compare with this instance, or <code>null</code>.</param>
        /// <returns>A signed number indicating the relative values of this instance and <code>other</code>.</returns>
        public int CompareTo(Money other)
        {
            if (!HasSameCurrencyAs(other))
                throw new ArgumentException("Compare is not defined between different currencies");

            return _amount.CompareTo(other._amount);
        }

        //  TODO: Provide some currency-dependent formatting. Java 1.4 Currency doesn't do it.
        //  public String formatString() {
        //      return currency.formatString(amount());
        //  }
        //  public String localString() {
        //      return currency.getFormat().format(amount());
        //  }
        #endregion


        #region IEquatable implementation
        /// <summary>
        /// Returns a value indicating whether this instance and a specified <seealso cref="Object"/> represent the same type and value.
        /// </summary>
        /// <param name="other">An object to compare to this instance.</param>
        /// <returns><code>true</code> if <code>other</code> is equal to this instance; otherwise, <code>false</code>.</returns>
        public override bool Equals(Object other)
        {
            try
            {
                return Equals((Money)other);
            }
            catch (InvalidCastException)
            {
                return false;
            }

        }

        /// <summary>
        /// Returns a value indicating whether this instance and a specified <code>Money</code> represent the same type and value.
        /// </summary>
        /// <param name="other">An object to compare to this instance.</param>
        /// <returns><code>true</code> if <code>other</code> is equal to this instance; otherwise, <code>false</code>.</returns>
        public bool Equals(Money other)
        {
            return
                other != null &&
                HasSameCurrencyAs(other) &&
                _amount.Equals(other._amount);
        }
        #endregion

        #region Privates
        private bool HasSameCurrencyAs(Money other)
        {
            return _currency.Equals(other._currency);
        }

        private void AssertHasSameCurrencyAs(Money aMoney)
        {
            if (!HasSameCurrencyAs(aMoney))
                throw new ArgumentException(aMoney.ToString() + " is not same currency as " + this.ToString());
        }
        #endregion

        #region Public overrides
        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return _amount.GetHashCode();
        }

        /// <summary>
        /// Returns a <seealso cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <seealso cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            var displayCulture = Thread.CurrentThread.CurrentCulture;
            return DisplayIn(displayCulture, false);
        }

        /// <summary>
        /// Returns a <seealso cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <seealso cref="System.String"/> that represents this instance.</returns>
        public string ToString(CultureInfo cultureInfo)
        {
            //var displayCulture = Thread.CurrentThread.CurrentCulture;
            return DisplayIn(cultureInfo);
        }

        /// <summary>
        /// Displays the current instance as it would appear in the native culture,
        /// no matter 'where' the context thread is running.
        /// </summary>
        /// <returns></returns>
        public string DisplayNative()
        {
            return DisplayIn(_currency.GetDisplayCulture());
        }

        /// <summary>
        /// Displays the current instance as it would appear in a specified culture.
        /// </summary>
        /// <param name="displayCulture">The display culture.</param>
        /// <returns></returns>
        public string DisplayIn(CultureInfo displayCulture)
        {
            return DisplayIn(displayCulture, true);
        }

        /// <summary>
        /// Displays the value of this instance in a non-native culture, preserving
        /// the characteristics of the native <see cref="CultureInfo" /> but respecting 
        /// target cultural formatting.
        /// </summary>
        /// <param name="displayCulture">The culture to display this money in</param>
        /// <param name="disambiguateMatchingSymbol">If <code>true</code>, if the native culture uses the same currency symbol as the display culture, the ISO currency code is appended to the value to help differentiate the native currency.</param>
        /// <returns>A value representing this instance in another culture</returns>
        public string DisplayIn(CultureInfo displayCulture, bool disambiguateMatchingSymbol)
        {
            var sb = new StringBuilder();

            var nativeCulture = _currency.GetDisplayCulture();
            if (displayCulture.Equals(nativeCulture))
            {
                disambiguateMatchingSymbol = false;
            }
            
            var nativeNumberFormat = nativeCulture.NumberFormat;
            nativeNumberFormat = (NumberFormatInfo)nativeNumberFormat.Clone();

            var displayNumberFormat = displayCulture.NumberFormat;
            nativeNumberFormat.CurrencyGroupSeparator = displayNumberFormat.CurrencyGroupSeparator;
            nativeNumberFormat.CurrencyDecimalSeparator = displayNumberFormat.CurrencyDecimalSeparator;

            sb.Append(_amount.ToString("c", nativeNumberFormat));

            // If the currency symbol of the display culture matches this money, add the code
            if (disambiguateMatchingSymbol && nativeNumberFormat.CurrencySymbol.Equals(displayNumberFormat.CurrencySymbol))
            {
                var currencyCode = new RegionInfo(nativeCulture.LCID).ISOCurrencySymbol;
                sb.Append(" ").Append(currencyCode);
            }

            return sb.ToString();
        }
        #endregion
    }
}
