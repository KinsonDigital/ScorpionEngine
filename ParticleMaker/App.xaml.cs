using KDParticleEngine;
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
        public App()
        {
            DIContainer = new Container();

            DIContainer.Register<IContentDirectoryService, ContentDirectoryService>();
            DIContainer.Register<ParticleTextureLoader>();
            DIContainer.Register<ParticleEngine>();
            DIContainer.RegisterInstance<IGraphicsEngineFactory>(new GraphicsEngineFactory());
            DIContainer.Register<GraphicsEngine>();
            DIContainer.Register<MainViewModel>();
        }


        public static Container DIContainer { get; set; }
    }
}
