using System.Collections.ObjectModel;
using System.Windows.Media;

namespace ParticleMaker.DesignData
{
    /// <summary>
    /// Provides sample data for the use at design time for user controls.
    /// </summary>
    public class ColorListSampleData
    {
        #region Props
        /// <summary>
        /// A list of color data.
        /// </summary>
        public ObservableCollection<ColorItem> Colors { get; set; }

        /// <summary>
        /// A color value.
        /// </summary>
        public SolidColorBrush ColorValue { get; set; }
        #endregion
    }
}
