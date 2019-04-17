using System;
using System.Globalization;
using System.Windows.Data;

namespace ParticleMaker.ValueConverters
{
    /// <summary>
    /// Converts the incoming string data to a boolean result. Empty or null will result
    /// in false.  Anything else will result in true.
    /// </summary>
    public class StringToBoolConverter : IValueConverter
    {
        #region Public Methods
        /// <summary>
        /// Converts the given <paramref name="value"/> of the given <paramref name="targetType"/>
        /// to a bool result.
        /// </summary>
        /// <param name="value">The incoming value.</param>
        /// <param name="targetType">The type of data incomging into the method.</param>
        /// <param name="parameter">The optional parameter data.</param>
        /// <param name="culture">The culture setting of the incoming data.</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !string.IsNullOrEmpty(value as string);


        /// <summary>
        /// Returns the given <paramref name="value"/> of the given <paramref name="targetType"/> unchanged.
        /// </summary>
        /// <param name="value">The incoming value.</param>
        /// <param name="targetType">The type of data incoming into the method.</param>
        /// <param name="parameter">The optional parameter data.</param>
        /// <param name="culture">The culture setting of the incoming data.</param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => false;
        #endregion
    }
}
