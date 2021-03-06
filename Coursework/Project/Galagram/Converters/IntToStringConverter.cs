using System.Windows.Data;

namespace Galagram.Converters
{
    /// <summary>
    /// Converts integer value to short string format
    /// </summary>
    [ValueConversion(sourceType: typeof(int), targetType: typeof(string))]
    public class IntToStringConverter : IValueConverter
    {
        // INNER TYPES
        private enum IntType
        {
            Thousand = 3,
            Million = 6,
            Billion = 9
        }

        // METHODS
        /// <summary>
        /// Converts integer value to short string format
        /// </summary>
        /// <param name="value">
        /// An integer value
        /// </param>
        /// <param name="targetType">
        /// The type of the binding target property.
        /// </param>
        /// <param name="parameter">
        /// The converter parameter to use.
        /// </param>
        /// <param name="culture">
        /// The culture to use in the converter
        /// </param>
        /// <returns>
        /// Short string value or empty string if <paramref name="value"/> is null
        /// </returns>
        /// <exception cref="System.FormatException">
        /// <paramref name="value"/> is not in an appropriate format.
        /// </exception>
        /// <exception cref="System.InvalidCastException">
        /// <paramref name="value"/> does not implement the <see cref="System.IConvertible"/> interface. -or- The conversion is not supported
        /// </exception>
        /// <exception cref="System.OverflowException">
        /// <paramref name="value"/> represents a number that is less than <see cref="System.Int32.MinValue"/> or greater than <see cref="System.Int32.MaxValue"/>
        /// </exception>
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return string.Empty;

            int number = System.Convert.ToInt32(value);

            if (number < 1000) return number.ToString();
            else if (number < 1000000) return string.Format("{0}K", GetFirstDigit(number, IntType.Thousand));
            else if (number < 1000000000) return string.Format("{0}M", GetFirstDigit(number, IntType.Million));
            else return string.Format("{0}B", GetFirstDigit(number, IntType.Billion));
        }
        /// <summary>
        /// Not expected behavior.
        /// </summary>
        /// <param name="value">The value that the binding target produces.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns> A values that have been converted from the target value back to the source value.</returns>
        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        private int GetFirstDigit(int number, IntType intType)
        {
            number = System.Math.Abs(number);

            for (int i = 0; i < (int)intType && number >= 10; ++i)
            {
                number /= 10;
            }

            return number;
        }
    }
}
