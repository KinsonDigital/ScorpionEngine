// <copyright file="GameScene.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Scene
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Numerics;
    using KDScorpionEngine.Entities;
    using KDScorpionEngine.Graphics;
    using Raptor.Content;

    /// <summary>
    /// A game scene within a game that can hold various game entities and game related logic.
    /// </summary>
    public abstract class GameScene : IScene
    {
        /// <summary>
        /// Creates a enw instance of <see cref="GameScene"/>.
        /// </summary>
        /// <param name="gravity">The gravity of the scene.</param>
        [ExcludeFromCodeCoverage]
        public GameScene(Vector2 gravity)
        {
        }

        /// <summary>
        /// Gets or sets the name of the scene.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the ID of this <see cref="GameScene"/>.
        /// </summary>
        public int Id { get; set; } = -1;

        /// <summary>
        /// Gets or sets a value indicating that the scene's content has been loaded.
        /// </summary>
        public bool ContentLoaded { get; set; }

        /// <summary>
        /// Gets or sets the time manager that manages the scene's frame timing and run mode.
        /// </summary>
        public ITimeManager TimeManager { get; set; } = new SceneTimeManager();

        /// <summary>
        /// Gets a value indicating if the scene has already been initialized.  Base method <see cref="GameScene.Initialize()"/>
        /// must be called for this functionality to work. Example: Use base.Initialize() in child class Initalize() method.
        /// </summary>
        public bool Initialized { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating if the scene is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the scene is currently rendering.
        /// </summary>
        public bool IsRenderingScene { get; set; }

        /// <summary>
        /// Gets the list of entities added to this <see cref="GameScene"/>.
        /// </summary>
        public List<IEntity> Entities { get; } = new List<IEntity>();

        /// <summary>
        /// Initializes the game scene.
        /// </summary>
        public virtual void Initialize()
        {
            Entities.ForEach(e =>
            {
                e.Init();
            });

            Initialized = true;
        }

        /// <summary>
        /// Loads all content using the given <paramref name="contentLoader"/>.
        /// </summary>
        /// <param name="contentManager">The content loader to use for loading and unloading the scene's content.</param>
        public virtual void LoadContent(IContentLoader contentLoader)
        {
            Entities.ForEach(e =>
            {
                e.LoadContent(contentLoader);
            });

            ContentLoaded = true;
        }

        /// <summary>
        /// Unloads all content using the given <paramref name="contentLoader"/>.
        /// </summary>
        /// <param name="contentManager">The content loader to use for loading and unloading the scene's content.</param>
        public virtual void UnloadContent(IContentLoader contentLoader)
        {
            Entities.ForEach(e =>
            {
                e.UnloadContent(contentLoader);
            });

            ContentLoaded = false;
        }

        /// <summary>
        /// Updates the <see cref="GameScene"/>.
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
            TimeManager?.Update(gameTime);

            // Update all of the entities
            Entities.ForEach(e => e.Update(gameTime));
        }

        /// <summary>
        /// Renders the <see cref="GameScene"/>.
        /// </summary>
        /// <param name="renderer">The renderer to use to render the scene.</param>
        public virtual void Render(IRenderer renderer)
        {
            Entities.ForEach(e =>
            {
                renderer.Render(e);

                // Call the render on the entity itself
                // The entity might contain its own entities
                e.Render(renderer);
            });

            IsRenderingScene = false;
        }

        // TODO: Make this class IEnumarable so we get the benefits of generics and IList functionality
        // in the class itself.
        public void AddEntity(Entity entity, bool addToPhysics = true) =>
            // TODO: Get this working
            // if(addToPhysics)
            //    PhysicsWorld.AddBody(entity.Body);

            Entities.Add(entity);
    }
}
