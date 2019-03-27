using ParticleMaker.UserControls;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
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
        public PathItem[] ProjectSetups { get; set; }

        /// <summary>
        /// Sets the currently open project that is used as the command parameter for the
        /// open project command in the maind window.
        /// </summary>
        public string CurrentOpenProject { get; set; }

        /// <summary>
        /// Gets or sets the title for the MainWindow.
        /// </summary>
        public string WindowTitle { get; set; }

        /// <summary>
        /// Gets or sets the setup item selected property data for the setup list box.
        /// </summary>
        public ICommand SetupItemSelected { get; set; }

        /// <summary>
        /// Gets or sets the AddSetup command test data for the setup list control in the main window.
        /// </summary>
        public ICommand AddSetup { get; set; }

        /// <summary>
        /// Create a new project command for the <see cref="MainWindow"/>.
        /// </summary>
        public ICommand NewProject { get; set; }

        /// <summary>
        /// Create a open project command for the <see cref="MainWindow"/>.
        /// </summary>
        public ICommand OpenProject { get; set; }

        /// <summary>
        /// Create a rename project command for the <see cref="MainWindow"/>.
        /// </summary>
        public ICommand RenameProject { get; set; }

        /// <summary>
        /// Create a delete project command for the <see cref="MainWindow"/>.
        /// </summary>
        public ICommand DeleteProject { get; set; }

        /// <summary>
        /// Create a close project command for the <see cref="MainWindow"/>.
        /// </summary>
        public ICommand CloseProject { get; set; }

        /// <summary>
        /// Gets the width to be used at design time for the <see cref="WindowsFormHost"/> control.
        /// </summary>
        public int RenderSurfaceWidth { get; } = 400;

        /// <summary>
        /// Gets the height to be used at design time for the <see cref="WindowsFormHost"/> control.
        /// </summary>
        public int RenderSurfaceHeight { get; } = 400;
        #endregion
    }
}
