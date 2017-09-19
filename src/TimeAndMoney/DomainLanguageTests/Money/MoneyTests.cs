using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DomainLanguageTests.Money
{
    public class MoneyTests
    {
        private static Currency USD = Currency.getInstance("USD");
        private static Currency JPY = Currency.getInstance("JPY");
        private static Currency EUR = Currency.getInstance("EUR");

        private Money d15;
        private Money d2_51;
        private Money y50;
        private Money e2_51;
        private Money d100;

        public MoneyTests()
        {
            d15 = Money.valueOf(new BigDecimal("15.0"), USD);
            d2_51 = Money.valueOf(new BigDecimal("2.51"), USD);
            e2_51 = Money.valueOf(new BigDecimal("2.51"), EUR);
            y50 = Money.valueOf(new BigDecimal("50"), JPY);
            d100 = Money.valueOf(new BigDecimal("100.0"), USD);

        }

        [Fact]
        public void MyTestMethod()
        {

        }
    }
}
