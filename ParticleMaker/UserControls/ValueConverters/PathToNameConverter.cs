using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace ParticleMaker.UserControls.ValueConverters
{
    public class PathToNameConverter : IValueConverter
    {
        #region Public Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = value as string;

            if (string.IsNullOrEmpty(path) || !Path.HasExtension(path))
                return "";


            return Path.GetFileNameWithoutExtension(path);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value as string;
        }
        #endregion
    }
}
