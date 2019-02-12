using KDParticleEngine;
using KDParticleEngine.Services;
using ParticleMaker.Project;
using ParticleMaker.Services;
using ParticleMaker.ViewModels;
using SimpleInjector;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows;

[assembly: InternalsVisibleTo(assemblyName: "ParticleMaker.Tests", AllInternalsVisible = true)]

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
            DIContainer.Register<IContentDirectoryService, ContentDirectoryService>();
            DIContainer.Register<IRandomizerService, RandomizerService>();
            DIContainer.Register<ParticleTextureLoader>();
            DIContainer.Register<ParticleEngine>();
            DIContainer.Register<ICoreEngine, CoreEngine>();
            DIContainer.Register<IGraphicsEngineFactory, GraphicsEngineFactory>();
            DIContainer.Register<GraphicsEngine>();
            DIContainer.Register<ProjectSettingsManager>();
            DIContainer.Register<SetupManager>();
            DIContainer.Register<MainViewModel>();
        }
        #endregion


        #region Props
        /// <summary>
        /// The dependency injection container for creating instances of registered objects.
        /// </summary>
        public static Container DIContainer { get; set; }
        #endregion


        #region Private Methods
        /// <summary>
        /// Starts up the application.
        /// </summary>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow();

            mainWindow.Show();
        }
        #endregion
    }
}
