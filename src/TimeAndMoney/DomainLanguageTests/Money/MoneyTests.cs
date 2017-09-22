﻿using CSharp.Util.Currency;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DomainLanguage.Money.Tests
{
    public class MoneyTests
    {
        private static Currency USD = Currency.GetInstance("USD");
        private static Currency JPY = Currency.GetInstance("JPY");
        private static Currency EUR = Currency.GetInstance("EUR");

        private Money d15;
        private Money d2_51;
        private Money y50;
        private Money e2_51;
        private Money d100;

        public MoneyTests()
        {
            d15 = Money.ValueOf(decimal.Parse("15.0"), USD);
            d2_51 = Money.ValueOf(decimal.Parse("2.51"), USD);
            e2_51 = Money.ValueOf(decimal.Parse("2.51"), EUR);
            y50 = Money.ValueOf(decimal.Parse("50"), JPY);
            d100 = Money.ValueOf(decimal.Parse("100.0"), USD);

        }

        [Fact]
        public void TestCreationFromDouble()
        {
            Assert.Equal(d15, Money.ValueOf(15.0, USD));
            Assert.Equal(d2_51, Money.ValueOf(2.51, USD));
            Assert.Equal(y50, Money.ValueOf(50.1, JPY));
            Assert.Equal(d100, Money.ValueOf(100D, USD));
        }

        [Fact]
        public void TestYen()
        {
            Assert.Equal("JPY 50", y50.ToString());
            Money y80 = Money.ValueOf(decimal.Parse("80"), JPY);
            Money y30 = Money.ValueOf(30D, JPY);
            Assert.Equal(y80, y50.Plus(y30));
            Assert.Equal(y80, y50.Times(new decimal(1.6)));
        }

        [Fact]
        public void TestMultiply()
        {
            Assert.Equal(Money.Dollars(150D), d15.Times(10));
            Assert.Equal(Money.Dollars(1.5D), d15.Times(0.1));
            Assert.Equal(Money.Dollars(70D), d100.Times(0.7));
        }

        [Fact]
        public void TestMultiplyRounding()
        {
            Assert.Equal(Money.Dollars(66.67), d100.Times(0.66666667));
            Assert.Equal(Money.Dollars(66.67), d100.Times(0.66666667, MidpointRounding.AwayFromZero));
        }

        [Fact]
        public void TestMultiplicationWithExplicitRounding()
        {
            Assert.Equal(Money.Dollars(66.67), d100.Times(decimal.Parse("0.666666"), MidpointRounding.ToEven));
            Assert.Equal(Money.Dollars(66.67), d100.Times(decimal.Parse("0.666666"), MidpointRounding.AwayFromZero));
            Assert.Equal(Money.Dollars(-66.67), d100.Negated().Times(decimal.Parse("0.666666"), MidpointRounding.AwayFromZero));
        }

        [Fact]
        public void TestDifferentCurrencyNotEqual()
        {
            Assert.True(!d2_51.Equals(e2_51));
        }

        [Fact]
        public void TestEquals()
        {
            Money d2_51a = Money.Dollars(2.51);
            Assert.Equal(d2_51a, d2_51);
        }

        [Fact]
        public void TestEqualsNull()
        {
            Money d2_51a = Money.Dollars(2.51);
            Object objectNull = null;
            Assert.False(d2_51a.Equals(objectNull));

            //This next test seems just like the previous, but it's not
            //The Java Compiler early binds message sends and
            //it will bind the next call to equals(Money) and
            //the previous will bind to equals(Object)
            //I renamed the original equals(Money) to
            //equalsMoney(Money) to prevent wrong binding.
            Money moneyNull = null;
            Assert.False(d2_51a.Equals(moneyNull));
        }

        [Fact]
        public void TestHash()
        {
            Money d2_51a = Money.Dollars(2.51);
            Assert.Equal(d2_51a.GetHashCode(), d2_51.GetHashCode());
        }

        [Fact]
        public void TestNegation()
        {
            Assert.Equal(Money.Dollars(-15), d15.Negated());
            Assert.Equal(e2_51, e2_51.Negated().Negated());
        }
    }
}
