// <copyright file="IScene.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Scene
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using KDScorpionEngine.Entities;
    using Raptor.Content;

    /// <summary>
    /// Represents a single scene.
    /// </summary>
    public interface IScene : IUpdatableObject, IDrawableObject
    {
        /// <summary>
        /// Gets or sets the name of the scene.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the scene's content has been loaded.
        /// </summary>
        bool ContentLoaded { get; set; }

        /// <summary>
        /// Gets or sets the time manager that manages the scene's frame timing and run mode.
        /// </summary>
        ITimeManager TimeManager { get; set; }

        /// <summary>
        /// Gets a value indicating whether the scene has already been initialized.
        /// </summary>
        /// <remarks>
        ///     The base method <see cref="GameScene.Initialize()"/> must be called for
        ///     this functionality to work.
        /// <para>
        ///     Example: Use base.Initialize() in child class Initalize() method.
        /// </para>
        /// </remarks>
        bool Initialized { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the scene is active.
        /// </summary>
        bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the scene is currently being rendered.
        /// </summary>
        bool IsRenderingScene { get; set; }

        /// <summary>
        /// Gets or sets the ID of the scene.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets the list of entities that are added to the scene.
        /// </summary>
        ReadOnlyCollection<IEntity> Entities { get; }

        /// <summary>
        /// Initializes the scene.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Loads all content for the scene using the given <paramref name="contentLoader"/>.
        /// </summary>
        /// <param name="contentLoader">The content loader to use for loading the scene's content.</param>
        void LoadContent(IContentLoader contentLoader);

        /// <summary>
        /// Unloads all content for the scene using the given <paramref name="contentLoader"/>.
        /// </summary>
        /// <param name="contentLoader">The content loader to use for unloading the scene's content.</param>
        void UnloadContent(IContentLoader contentLoader);
    }
}
