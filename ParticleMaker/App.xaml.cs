﻿using KDParticleEngine;
using ParticleMaker.Services;
using ParticleMaker.ViewModels;
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
            DIContainer.Register<ParticleEngine>();
            DIContainer.RegisterInstance<IGraphicsEngineFactory>(new GraphicsEngineFactory());
            DIContainer.Register<GraphicsEngine>();
            DIContainer.Register<MainViewModel>();
        }


        public static Container DIContainer { get; set; }
    }
}
