// <copyright file="IoC.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine
{
    using System;
    using System.IO.Abstractions;
    using Raptor.Services;
    using SimpleInjector;

    internal static class IoC
    {
        private static readonly FileSystem FileSystem = new FileSystem();
        private static Container? container;

        public static void Init()
        {
            container = new Container();

            Container.Register(() => FileSystem.File, Lifestyle.Singleton);
            Container.Register(() => FileSystem.Directory, Lifestyle.Singleton);

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
