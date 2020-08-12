// <copyright file="IScene.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Scene
{
    using Raptor.Content;

    /// <summary>
    /// Represents a single scene.
    /// </summary>
    public interface IScene : IDrawable, IUpdatable
    {
        /// <summary>
        /// Gets or sets the name of the scene.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating that the scene's content has been loaded.
        /// </summary>
        bool ContentLoaded { get; set; }

        /// <summary>
        /// Gets or sets the time manager used for this scene.
        /// </summary>
        ITimeManager TimeManager { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the scene is active.
        /// </summary>
        bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the scene is currently in the rendering state.
        /// </summary>
        bool IsRenderingScene { get; set; }

        /// <summary>
        /// Gets or sets the ID of the scene.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Initializes the scene.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Loads all content for the scene using the given <paramref name="contentLoader"/>.
        /// </summary>
        /// <param name="contentManager">The content loader to use for loading and unloading the scene's content.</param>
        void LoadContent(ContentLoader contentLoader);

        /// <summary>
        /// Unloads all content for the scene using the given <paramref name="contentLoader"/>.
        /// </summary>
        /// <param name="contentManager">The content loader to use for loading and unloading the scene's content.</param>
        void UnloadContent(ContentLoader contentLoader);
    }
}
