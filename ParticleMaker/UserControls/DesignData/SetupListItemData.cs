using System.Windows.Media;

namespace ParticleMaker.UserControls.DesignData
{
    /// <summary>
    /// Used for design data for the <see cref="SetupListItem"/> user control
    /// </summary>
    public class SetupListItemData
    {
        #region Props
        /// <summary>
        /// Gets or sets the setup path.
        /// </summary>
        public string SetupPath { get; set; }

        /// <summary>
        /// Gets or sets the name of the setup.
        /// </summary>
        public string SetupName { get; set; }

        /// <summary>
        /// Gets or sets the brush for the error border.
        /// </summary>
        public SolidColorBrush ErrorBorderBrush { get; set; }
        #endregion
    }
}
