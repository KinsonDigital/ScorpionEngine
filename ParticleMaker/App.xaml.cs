﻿using System.Windows;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using SimpleInjector;
using KDParticleEngine;
using KDParticleEngine.Services;
using ParticleMaker.Exceptions;
using ParticleMaker.Management;
using ParticleMaker.Services;
using ParticleMaker.ViewModels;

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
            DIContainer.Register<IRandomizerService, RandomizerService>();
            DIContainer.Register<ParticleEngine>();
            DIContainer.Register<ICoreEngine, CoreEngine>();
            DIContainer.Register<IGraphicsEngineFactory, GraphicsEngineFactory>();
            DIContainer.Register<GraphicsEngine>();

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
        #endregion


        #region Private Methods
        /// <summary>
        /// Starts up the application.
        /// </summary>
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
