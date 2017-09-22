/*
 * Copyright (c) 2004 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 */
using CSharp.Util.Currency;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLanguage.Money
{
    public class Money : IEquatable<Money>, IComparable<Money>
    {
        private static readonly Currency CAD = Currency.GetInstance("CAD");
        private static readonly Currency EUR = Currency.GetInstance("EUR");
        private static readonly Currency USD = Currency.GetInstance("USD");
        private static readonly MidpointRounding DEFAULT_ROUNDING_MODE = MidpointRounding.AwayFromZero;

        private decimal amount;
        private Currency currency;

        #region Publics
        /// <summary>
        /// The constructor does not complex computations and requires simple, 
        /// inputs consistent with the class invariant. 
        /// Other creation methods are available for convenience.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        public Money(decimal amount, Currency currency)
        {
            this.currency = currency;
            this.amount = decimal.Round(amount, currency.GetDefaultFractionDigits(), MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// This creation method is safe to use. It will adjust scale, but will not round off the amount.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public static Money ValueOf(decimal amount, Currency currency)
        {
            return Money.ValueOf(amount, currency, MidpointRounding.AwayFromZero);
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
        public static Money ValueOf(double dblAmount, Currency currency)
        {
            return Money.ValueOf(dblAmount, currency, DEFAULT_ROUNDING_MODE);
        }

        /// <summary>
        /// Because of the indefinite precision of double, this method must round off the value. This method gives the client control of the rounding mode.
        /// </summary>
        /// <param name="dblAmount"></param>
        /// <param name="currency"></param>
        /// <param name="roundingMode"></param>
        /// <returns></returns>
        public static Money ValueOf(double dblAmount, Currency currency, MidpointRounding roundingMode)
        {
            decimal rawAmount = new decimal(dblAmount);
            return Money.ValueOf(rawAmount, currency, roundingMode);
        }


        /// <summary>
        /// WARNING: Because of the indefinite precision of double, thismethod must round off the value.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static Money Dollars(double amount)
        {
            return Money.ValueOf(amount, USD);
        }

        /// <summary>
        /// This creation method is safe to use. It will adjust scale, but will not round off the amount.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static Money Dollars(decimal amount)
        {
            return Money.ValueOf(amount, USD);
        }

        public static Money Dollars(int amount)
        {
            return Money.ValueOf(new decimal(amount), USD);
        }
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
            return amount;
        }

        public Currency BreachEncapsulationOfCurrency()
        {
            return currency;
        }

        public Money Negated()
        {
            return Money.ValueOf(decimal.Negate(amount), currency);
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
            return Money.ValueOf(decimal.Multiply(amount, factor), currency, roundingMode);
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
            return Money.ValueOf(decimal.Add(amount, other.amount), currency);
        }
        #endregion

        #region IComparable implementation
        public int CompareTo(Money other)
        {
            if (!hasSameCurrencyAs(other))
                throw new ArgumentException("Compare is not defined between different currencies");

            return amount.CompareTo(other.amount);
        }
        #endregion

        #region IEquatable implementation
        public bool Equals(Money other)
        {
            return
                other != null &&
                hasSameCurrencyAs(other) &&
                amount.Equals(other.amount);
        }
        #endregion

        #region Privates
        private bool hasSameCurrencyAs(Money other)
        {
            return currency.Equals(other.currency);
        }


        private void AssertHasSameCurrencyAs(Money aMoney)
        {
            if (!hasSameCurrencyAs(aMoney))
                throw new ArgumentException(aMoney.ToString() + " is not same currency as " + this.ToString());
        }
        #endregion

        #region Public overrides
        public override int GetHashCode()
        {
            return amount.GetHashCode();
        }

        public override string ToString()
        {
            //TODO change GetCurrencyCode for GetSymbol when it is implemented in CSharp.Util.Currency
            return currency.GetCurrencyCode() + " " + amount;
        }
        #endregion
    }
}
