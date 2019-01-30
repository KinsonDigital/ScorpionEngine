using System.Windows.Media;

namespace ParticleMaker.UserControls.DesignData
{
    /// <summary>
    /// Used for design data for the various user controls.
    /// </summary>
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
        public SetupPathItem[] Setups { get; set; } = new SetupPathItem[]
        {
            new SetupPathItem() { FilePath = @"C:\temp\test-setup\setup1.json" },
            new SetupPathItem() { FilePath = @"C:\temp\test-setup\setup2.json" },
            new SetupPathItem() { FilePath = @"C:\temp\test-setup\setup3.json" }
        };

        /// <summary>
        /// Gets or sets the error message during design time.
        /// </summary>
        public string ErrorMessage { get; set; } = "This is a test error.";

        /// <summary>
        /// Gets or sets the name of the particle.
        /// </summary>
        public string ParticleName { get; set; }
        #endregion
    }
}
