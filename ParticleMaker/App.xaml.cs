using System.Windows;
using System.Diagnostics.CodeAnalysis;
using SimpleInjector;
using KDParticleEngine;
using KDParticleEngine.Services;
using ParticleMaker.Exceptions;
using ParticleMaker.Management;
using ParticleMaker.Services;
using ParticleMaker.ViewModels;
using ParticleMaker.UserControls;//KEEP THIS. SHOWS USED WHEN IN UserControlTesting mode
using System.Reflection;
using System.Diagnostics;

namespace ParticleMaker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class App : Application
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="App"/>.
        /// </summary>
        public App()
        {
            DIContainer = new Container();

            DIContainer.Register<IDirectoryService, DirectoryService>();
            DIContainer.Register<IFileService, JSONFileService>();
            DIContainer.Register<ProjectIOService>();
            DIContainer.Register<IRandomizerService, RandomizerService>();
            DIContainer.Register<ParticleEngine<ParticleTexture>>();
            DIContainer.Register<IRenderer, SDLRenderer>();
            DIContainer.Register<RenderEngine>();
            DIContainer.Register<ITimingService, TimingService>();
            DIContainer.Register<ITaskManagerService, TaskManagerService>();

            DIContainer.Register<ProjectSettingsManager>();
            DIContainer.Register<SetupManager>();
            DIContainer.Register<MainViewModel>();

            //Setup the exception handler
            ExceptionHandler.LoggingEnabled = true;
            ExceptionHandler.Logger = new JSONLoggerService(DIContainer.GetInstance<IDirectoryService>(), DIContainer.GetInstance<IFileService>());
            ExceptionHandler.ShowMessageBoxEnabled = true;
            ExceptionHandler.ExceptionMessageBox = new ExceptionMessage();
        }
        #endregion


        #region Props
        /// <summary>
        /// The dependency injection container for creating instances of registered objects.
        /// </summary>
        public static Container DIContainer { get; set; }

        /// <summary>
        /// Gets the current version of the application.
        /// </summary>
        public static string Version => $"v{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion}";

        /// <summary>
        /// Gets a value indicating if the application is shutting down.
        /// </summary>
        public static bool IsShuttingDown { get; set; }
        #endregion


        #region Private Methods
        /// <summary>
        /// Starts up the application.
        /// </summary>
        /// <remarks>Keep the reference of <see cref="UserControlTestWindow"/> in this comment to pretent deletion of using at top of the file.</remarks>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
#if USERCTRLTESTING
            //This is used for testing out user controls during runtime for debugging purposes
            var userCtrlTestWindow = new UserControlTestWindow();
            userCtrlTestWindow.Show();
#else
            var mainWindow = new MainWindow();
            mainWindow.Show();
#endif
        }
        #endregion
    }
}
