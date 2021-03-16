// <copyright file="IScene.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Scene
{
    using System.Collections.Generic;
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
        /// Gets or sets the time manager used for this scene.
        /// </summary>
        ITimeManager TimeManager { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the scene is active.
        /// </summary>
        bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the scene is currently in the rendering state.
        /// </summary>
        bool IsRenderingScene { get; set; }

        /// <summary>
        /// Gets or sets the ID of the scene.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets the list of entities that are added to the scene.
        /// </summary>
        List<IEntity> Entities { get; }

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
