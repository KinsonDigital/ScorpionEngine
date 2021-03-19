// <copyright file="IoC.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO.Abstractions;
    using KDScorpionEngine.Factories;
    using KDScorpionEngine.Input;
    using KDScorpionEngine.Utils;
    using Raptor.Input;
    using Raptor.Services;
    using SimpleInjector;

    /// <summary>
    /// An IoC (Inversion Of Control) container that provides for the use of DI (dependency injection).
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class IoC
    {
        private static readonly FileSystem FileSystem = new FileSystem();
        private static Container? container;
        private static bool isDisposed = true;

        /// <summary>
        /// Gets the IoC container for produces instances.
        /// </summary>
        public static Container Container
        {
            get
            {
                if (container is null)
                {
                    throw new Exception($"The '{nameof(IoC)}.{nameof(Container)}' container cannot be used until '{nameof(IoC.Init)}()' has been invoked.");
                }

                return container;
            }
        }

        /// <summary>
        /// Initializes the IoC container.
        /// </summary>
        public static void Init()
        {
            if (isDisposed is false)
            {
                return;
            }

            container = new Container();

            Container.Register(() => FileSystem.File, Lifestyle.Singleton);
            Container.Register(() => FileSystem.Directory, Lifestyle.Singleton);

            Container.Register<IImageFileService, ImageFileService>(Lifestyle.Singleton);

            Container.RegisterAndIgnoreDisposableWarnings<KeyboardWatcher>();
            Container.RegisterAndIgnoreDisposableWarnings<MouseWatcher>();

            Container.Register<IGameInput<KeyCode, KeyboardState>, Keyboard>();
            Container.Register<IGameInput<MouseButton, MouseState>, Mouse>();
            Container.Register<IStopWatch, StopWatch>();
            Container.Register<ICounter, Counter>();
            Container.Register<IEntityFactory, EntityFactory>(Lifestyle.Singleton);

            isDisposed = false;
        }

        /// <summary>
        /// Disposes of the container.
        /// </summary>
        public static void Dispose() => Dispose(true);

        /// <summary>
        /// Diposes of the container.
        /// </summary>
        /// <param name="disposing">
        ///     <see langword="true"/> to dispose of managed resources.
        /// </param>
        private static void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    Container.Dispose();
                }

                isDisposed = true;
            }
        }
    }
}
