// <copyright file="GameScene.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Scene
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using KDScorpionEngine.Entities;
    using KDScorpionEngine.Graphics;
    using Raptor.Content;

    /// <summary>
    /// A game scene that can hold various game entities and game related logic.
    /// </summary>
    public abstract class GameScene : IScene
    {
        private readonly List<IEntity> entities = new List<IEntity>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GameScene"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public GameScene()
        {
        }

        /// <inheritdoc/>
        public string Name { get; set; } = string.Empty;

        /// <inheritdoc/>
        public int Id { get; set; } = -1;

        /// <summary>
        /// Gets or sets a value indicating whether the scene's content has been loaded.
        /// </summary>
        public bool ContentLoaded { get; set; }

        /// <inheritdoc/>
        public ITimeManager TimeManager { get; set; } = new SceneTimeManager();

        /// <inheritdoc/>
        public bool Initialized { get; private set; }

        /// <inheritdoc/>
        public bool Active { get; set; }

        /// <inheritdoc/>
        public bool IsRenderingScene { get; set; }

        /// <inheritdoc/>
        public ReadOnlyCollection<IEntity> Entities => this.entities.ToReadOnlyCollection();

        /// <inheritdoc/>
        public virtual void Initialize()
        {
            for (var i = 0; i < Entities.Count; i++)
            {
                Entities[i].Init();
            }

            Initialized = true;
        }

        /// <summary>
        /// Loads all content using the given <paramref name="contentLoader"/>.
        /// </summary>
        /// <param name="contentLoader">Loads the scene's content.</param>
        public virtual void LoadContent(IContentLoader contentLoader)
        {
            for (var i = 0; i < Entities.Count; i++)
            {
                Entities[i].LoadContent(contentLoader);
            }

            ContentLoaded = true;
        }

        /// <summary>
        /// Unloads all content using the given <paramref name="contentLoader"/>.
        /// </summary>
        /// <param name="contentLoader">Unloads the scene's content.</param>
        public virtual void UnloadContent(IContentLoader contentLoader)
        {
            for (var i = 0; i < Entities.Count; i++)
            {
                Entities[i].UnloadContent(contentLoader);
            }

            ContentLoaded = false;
        }

        /// <summary>
        /// Updates the scene.
        /// </summary>
        /// <param name="gameTime">The time that has passed in the game.</param>
        public virtual void Update(GameTime gameTime)
        {
            TimeManager?.Update(gameTime);

            // Update all of the entities
            for (var i = 0; i < Entities.Count; i++)
            {
                Entities[i].Update(gameTime);
            }
        }

        /// <summary>
        /// Renders the scenes <see cref="Entities"/>.
        /// </summary>
        /// <param name="renderer">Renders entities.</param>
        public virtual void Render(IRenderer renderer)
        {
            for (var i = 0; i < Entities.Count; i++)
            {
                renderer.Render(Entities[i]);

                // Call the render on the entity itself
                // The entity might contain its own entities
                Entities[i].Render(renderer);
            }

            IsRenderingScene = false;
        }

        /// <inheritdoc/>
        public void AddEntity(IEntity entity) => this.entities.Add(entity);
    }
}
