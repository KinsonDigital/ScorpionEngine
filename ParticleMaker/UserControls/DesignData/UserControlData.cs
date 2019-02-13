using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ParticleMaker.UserControls.DesignData
{
    /// <summary>
    /// Used for design data for the various user controls.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UserControlData
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

        /// <summary>
        /// Gets or sets the setup paths design data for the <see cref="SetupList"/> user control.
        /// </summary>
        public PathItem[] Setups { get; set; }

        /// <summary>
        /// Gets or sets the particle paths design data for the <see cref="ParticleList"/> user control.
        /// </summary>
        public PathItem[] Particles { get; set; }

        /// <summary>
        /// Gets or sets the error message during design time.
        /// </summary>
        public string ErrorMessage { get; set; } = "This is a test error.";

        /// <summary>
        /// Gets or sets the name of the particle.
        /// </summary>
        public string ParticleName { get; set; }

        /// <summary>
        /// Gets or sets the file path to the particle for the design data of the ParticleListItem control.
        /// </summary>
        public string ParticleFilePath { get; set; }

        /// <summary>
        /// The label text data for the <see cref="NumericUpDown"/> control.
        /// </summary>
        public string LabelText { get; set; } = nameof(LabelText);
        #endregion
    }
}
