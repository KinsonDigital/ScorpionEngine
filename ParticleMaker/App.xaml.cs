using ParticleMaker.Services;
using SimpleInjector;
using System.Windows;

namespace ParticleMaker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DIContainer = new Container();

            DIContainer.Register<IContentDirectoryService, ContentDirectoryService>();
            DIContainer.Register<ParticleTextureLoader>();
        }


        public static Container DIContainer { get; set; }
    }
}
