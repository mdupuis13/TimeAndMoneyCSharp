using CSharp.Util.Currency;
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
    }
}
