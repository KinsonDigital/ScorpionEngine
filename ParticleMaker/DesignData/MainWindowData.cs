using ParticleMaker.UserControls;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;

namespace ParticleMaker.DesignData
{
    /// <summary>
    /// Provides sample data for the use at design time for user controls.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MainWindowData
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

        /// <summary>
        /// Gets or sets the data for the <see cref="ParticleList"/> control.
        /// </summary>
        public PathItem[] Particles { get; set; }

        /// <summary>
        /// Gets or sets the data for the <see cref="SetupList"/> control.
        /// </summary>
        public PathItem[] Setups { get; set; }

        /// <summary>
        /// Get or sets the new project command for the File=>New Project menu item.
        /// </summary>
        public RelayCommand NewProject { get; set; }
        #endregion
    }
}
