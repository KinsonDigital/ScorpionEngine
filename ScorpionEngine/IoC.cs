using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;
using KDScorpionEngine.Content;
using KDScorpionEngine.Entities;
using Raptor.Content;
using Raptor.Graphics;
using Raptor.Services;
using SimpleInjector;

namespace KDScorpionEngine
{
    internal static class IoC
    {
        private static readonly FileSystem FileSystem = new FileSystem();
        private static Container? container;

        public static void Init()
        {
            container = new Container();

            Container.Register(() => FileSystem.File, Lifestyle.Singleton);
            Container.Register(() => FileSystem.Directory, Lifestyle.Singleton);
            Container.Register<ILoader<AtlasData>, KDScorpionEngine.Content.AtlasDataLoader>(Lifestyle.Singleton);
            Container.Register<ILoader<ITexture>, TextureLoader>(Lifestyle.Singleton);
            Container.Register<IContentSource, AtlasContentSource>(Lifestyle.Singleton);
            Container.Register<IContentSource, AtlasContentSource>(Lifestyle.Singleton);

            // TODO: This should have a factory in the Raptor lib called ServicesFactory for getting service instances
            Container.Register<IImageFileService, ImageFileService>(Lifestyle.Singleton);
        }

        public static Container Container
        {
            get
            {
                if (container is null)
                {
                    throw new Exception($"The '{nameof(IoC)}.{nameof(IoC.Container)}' container cannot be used until '{nameof(IoC.Init)}()' has been invoked.");
                }

                return container;
            }
        }
    }
}
