using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace ParticleMaker.ValueConverters
{
    /// <summary>
    /// Converts incoming data from a binding from a file path to a file name without
    /// the directories or file extension attached.
    /// </summary>
    public class PathToNameConverter : IValueConverter
    {
        #region Public Methods
        /// <summary>
        /// Converts the given <paramref name="value"/> of the given <paramref name="targetType"/>
        /// to a name.
        /// </summary>
        /// <param name="value">The incoming value.</param>
        /// <param name="targetType">The type of data incomging into the method.</param>
        /// <param name="parameter">The optional parameter data.</param>
        /// <param name="culture">The culture setting of the incoming data.</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = value as string;

            if (string.IsNullOrEmpty(path) || !Path.HasExtension(path))
                return "";


            return Path.GetFileNameWithoutExtension(path);
        }


        /// <summary>
        /// Returns the given <paramref name="value"/> of the given <paramref name="targetType"/> unchanged.
        /// </summary>
        /// <param name="value">The incoming value.</param>
        /// <param name="targetType">The type of data incoming into the method.</param>
        /// <param name="parameter">The optional parameter data.</param>
        /// <param name="culture">The culture setting of the incoming data.</param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value as string;
        #endregion
    }
}
