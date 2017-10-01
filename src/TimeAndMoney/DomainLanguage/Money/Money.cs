/*
 * Copyright (c) 2004 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 */
using CSharp.Util.Currency;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;

namespace DomainLanguage.Money
{
    public class Money : IEquatable<Money>, IComparable<Money>
    {
        private static readonly Currency CAD = Currency.GetByLetterCode("CAD");
        private static readonly Currency EUR = Currency.GetByLetterCode("EUR");
        private static readonly Currency USD = Currency.GetByLetterCode("USD");
        private static readonly MidpointRounding DEFAULT_ROUNDING_MODE = MidpointRounding.ToEven;

        private decimal _amount;
        private Currency _currency;

        #region Publics
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
            this._currency = Currency.GetByLetterCode(CultureInfo.CurrentCulture.ThreeLetterISOLanguageName);
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
        /// <param name="dblAmount"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public static Money ValueOf(double amount, Currency currency)
        {
            return Money.ValueOf(amount, currency, DEFAULT_ROUNDING_MODE);
        }

        /// <summary>
        /// Because of the indefinite precision of double, this method must round off the value. This method gives the client control of the rounding mode.
        /// </summary>
        /// <param name="dblAmount"></param>
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

        #endregion

        /*
         * How best to handle access to the internals? It is needed for
         * database mapping, UI presentation, and perhaps a few other
         * uses. Yet giving public access invites people to do the
         * real work of the Money object elsewhere.
         * Here is an experimental approach, giving access with a 
         * warning label of sorts. Let us know how you like it.
         */
        public decimal BreachEncapsulationOfAmount()
        {
            return _amount;
        }

        public Currency BreachEncapsulationOfCurrency()
        {
            return _currency;
        }

        public Money Negated()
        {
            return Money.ValueOf(decimal.Negate(_amount), _currency);
        }

        public Money Times(decimal factor)
        {
            return Times(factor, DEFAULT_ROUNDING_MODE);
        }

        /*
         * TODO: BigDecimal.multiply() scale is sum of scales of two multiplied numbers. So what is scale of times?
         */

        public Money Times(decimal factor, MidpointRounding roundingMode)
        {
            return Money.ValueOf(decimal.Multiply(_amount, factor), _currency, roundingMode);
        }

        public Money Times(double amount, MidpointRounding roundingMode)
        {
            return Times(new decimal(amount), roundingMode);
        }

        public Money Times(double amount)
        {
            return Times(new decimal(amount));
        }

        public Money Times(int i)
        {
            return Times(new decimal(i));
        }

        public Money Plus(Money other)
        {
            AssertHasSameCurrencyAs(other);
            return Money.ValueOf(decimal.Add(_amount, other._amount), _currency);
        }

        public decimal GetAmount()
        {
            return _amount;
        }

        public Currency GetCurrency()
        {
            return _currency;
        }

        public bool IsGreaterThan(Money other)
        {
            return (CompareTo(other) > 0);
        }

        public bool IsLessThan(Money other)
        {
            return (CompareTo(other) < 0);
        }

        public int CompareTo(Object other)
        {
            return CompareTo((Money)other);
        }

        // IComparable implementation
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
        public override int GetHashCode()
        {
            return _amount.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var displayCulture = Thread.CurrentThread.CurrentCulture;
            return DisplayIn(displayCulture, false);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
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
        /// the characteristics of the native <see cref="CurrencyInfo" /> but respecting 
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
