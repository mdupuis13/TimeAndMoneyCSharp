using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLanguage.Money
{
    public static class DecimalExtension
    {
        /// <summary>
        /// Returns thee number of digits. Counts only digits, excluding the decimal separator
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetPrecision(this decimal value)
        {
            return GetLeftNumberOfDigits(value) + GetRightNumberOfDigits(value);
        }

        /// <summary>
        /// Number of digits to the right of the decimal point without ending zeros
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetScale(this decimal value)
        {
            return GetRightNumberOfDigits(value);
        }

        /// <summary>
        /// Number of digits to the right of the decimal point without ending zeros
        /// </summary>
        /// <param name="value"></param>
        /// <returns>the number of digits to the right of the decimal separator</returns>
        public static int GetRightNumberOfDigits(this decimal value)
        {
            var text = value.ToString(System.Globalization.CultureInfo.InvariantCulture).TrimEnd('0');
            var decpoint = text.IndexOf(System.Globalization.CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);

            if (decpoint < 0)
                return 0;

            return text.Length - decpoint - 1;
        }

        /// <summary>
        /// Number of digits to the left of the decimal point without starting zeros
        /// </summary>
        /// <param name="value"></param>
        /// <returns>the number of digits to the left of the decimal separator</returns>
        public static int GetLeftNumberOfDigits(this decimal value)
        {
            var text = Math.Abs(value).ToString(System.Globalization.CultureInfo.InvariantCulture).TrimStart('0');
            var decpoint = text.IndexOf(System.Globalization.CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);

            if (decpoint == -1)
                return text.Length;

            return decpoint;
        }
    }
}
