/* Copyright (c) 2004 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 */
using FluentAssertions;
using System;
using Xunit;

namespace DomainLanguage.Base.Tests
{
    public class RatioTests
    {
        [Fact]
        public void TestDecimalRatio()
        {
            // 3/2
            Ratio r3over2 = Ratio.Of(new decimal(3), new decimal(2));
            decimal result = r3over2.DecimalValue(1, MidpointRounding.AwayFromZero);
            result.Should().Be(new decimal(1.5), "because 3 divided by 2 equals 1.5");

            // 10/3
            Ratio r10over3 = Ratio.Of(new decimal(10), new decimal(3));
            result = r10over3.DecimalValue(3, MidpointRounding.AwayFromZero);
            result.Should().Be(new decimal(3.333), "because we want 3 decimals");

            result = r10over3.DecimalValue(3, MidpointRounding.ToEven);
            result.Should().Be(new decimal(3.333), "because we want 3 decimals rounded to even");

            // 3
            Ratio r3 = Ratio.Of(new decimal(3));
            result = r3.DecimalValue(3, MidpointRounding.AwayFromZero);
            result.Should().Be(new decimal(3), "because we did not provide a denominator");

            Ratio rManyDigits = Ratio.Of(decimal.Parse("9.001"), new decimal(3));
            result = rManyDigits.DecimalValue(6, MidpointRounding.AwayFromZero);
            result.Should().Be(decimal.Parse("3.000333"), "because we want 3 decimals");

            result = rManyDigits.DecimalValue(6, MidpointRounding.ToEven);
            result.Should().Be(decimal.Parse("3.000333"), "because we want 3 decimals rounded to even");

            result = rManyDigits.DecimalValue(7, MidpointRounding.AwayFromZero);
            result.Should().Be(decimal.Parse("3.0003333"), "because we want 7 decimals");

            result = rManyDigits.DecimalValue(7, MidpointRounding.ToEven);
            result.Should().Be(decimal.Parse("3.0003333"), "because we want 7 decimals rounded to even");
        }

        [Fact]
        public void TestLongRatio()
        {
            // 3/2
            Ratio r3over2 = Ratio.Of(3, 2);
            decimal result = r3over2.DecimalValue(1, MidpointRounding.AwayFromZero);
            result.Should().Be(new decimal(1.5), "because 3 divided by 2 equals 1.5");

            // 10/3
            Ratio r10over3 = Ratio.Of(10, 3);
            result = r10over3.DecimalValue(3, MidpointRounding.AwayFromZero);
            result.Should().Be(new decimal(3.333), "because we want only 3 decimals");

            result = r10over3.DecimalValue(3, MidpointRounding.ToEven);
            result.Should().Be(new decimal(3.333), "because we want only 3 decimals rounded to even");

            // 2/3
            Ratio r2over3 = Ratio.Of(2, 3);
            result = r2over3.DecimalValue(3, MidpointRounding.AwayFromZero);
            result.Should().Be(new decimal(0.667), "because we want only 3 decimals");

            result = r2over3.DecimalValue(3, MidpointRounding.ToEven);
            result.Should().Be(new decimal(0.667), "because we want only 3 decimals rounded to even");

            // many digits
            Ratio rManyDigits = Ratio.Of(9001L, 3000L);
            result = rManyDigits.DecimalValue(6, MidpointRounding.AwayFromZero);
            result.Should().Be(decimal.Parse("3.000333"), "because we want only 3 decimals rounded to even");
            
        }

        [Fact]
        public void TestEquals()
        {
            Ratio.Of(100, 200).Equals(Ratio.Of(100, 200)).Should().BeTrue(); ;

            Ratio.Of(100, 200).Should().Be(Ratio.Of(100, 200));

            Ratio.Of(100, 200).Should().Be(Ratio.Of(decimal.Parse("100"), decimal.Parse("200")));
        }


        [Fact]
        public void TestMultiplyNumerator()
        {
            Ratio expected = Ratio.Of(decimal.Parse("9901.1"), new decimal(3000));

            Ratio rManyDigits = Ratio.Of(9001, 3000);
            Ratio product = rManyDigits.Times(decimal.Parse("1.1"));

            product.Should().Be(expected);
        }

        [Fact]
        public void TestMultiplyByRatio()
        {
            Ratio r1 = Ratio.Of(9001, 3000);
            Ratio r2 = Ratio.Of(3, 2);
            Ratio expectedProduct = Ratio.Of(27003, 6000);

            Ratio product = r1.Times(r2);
            product.Should().Be(expectedProduct);
        }

        [Fact]
        public void TestToString()
        {
            string expected = "9001/3000";

            Ratio r1 = Ratio.Of(9001, 3000);
            r1.ToString().Should().Be(expected);
        }
    }
}
