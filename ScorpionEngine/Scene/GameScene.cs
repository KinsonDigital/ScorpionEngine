using KDScorpionEngine.Entities;
using KDScorpionEngine.Graphics;
using Raptor;
using Raptor.Content;
using Raptor.Physics;
using Raptor.Plugins;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace KDScorpionEngine.Scene
{
    /// <summary>
    /// A game scene within a game that can hold various game entities and game related logic.
    /// </summary>
    public abstract class GameScene : IScene
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="GameScene"/>.
        /// USED FOR UNIT TESTING.
        /// </summary>
        /// <param name="physicsWorld">The mocked physics world to inject.</param>
        internal GameScene(IPhysicsWorld physicsWorld) => PhysicsWorld = new PhysicsWorld(physicsWorld);


        /// <summary>
        /// Creates a enw instance of <see cref="GameScene"/>.
        /// </summary>
        /// <param name="gravity">The gravity of the scene.</param>
        [ExcludeFromCodeCoverage]
        public GameScene(Vector2 gravity) => PhysicsWorld = new PhysicsWorld(gravity);
        #endregion


        #region Props
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
        public List<Entity> Entities { get; } = new List<Entity>();

        /// <summary>
        /// The physics world attached to this <see cref="GameScene"/> that governs the physics of the game.
        /// </summary>
        public static PhysicsWorld PhysicsWorld { get; set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Initializes the game scene.
        /// </summary>
        public virtual void Initialize()
        {
            PhysicsWorld.Update(0);

            Initialized = true;
        }


        /// <summary>
        /// Loads all content using the given <paramref name="contentLoader"/>.
        /// </summary>
        /// <param name="contentManager">The content loader to use for loading and unloading the scene's content.</param>
        public virtual void LoadContent(ContentLoader contentLoader) => ContentLoaded = true;


        /// <summary>
        /// Unloads all content using the given <paramref name="contentLoader"/>.
        /// </summary>
        /// <param name="contentManager">The content loader to use for loading and unloading the scene's content.</param>
        public virtual void UnloadContent(ContentLoader contentLoader) => ContentLoaded = false;


        /// <summary>
        /// Updates the <see cref="GameScene"/>.
        /// </summary>
        public virtual void Update(EngineTime engineTime)
        {
            TimeManager?.Update(engineTime);

            //Update all of the entities
            Entities.ForEach(e => e.Update(engineTime));

            //Update the physics world
            PhysicsWorld.Update((float)engineTime.ElapsedEngineTime.TotalSeconds);
        }


        /// <summary>
        /// Renders the <see cref="GameScene"/>.
        /// </summary>
        /// <param name="renderer">The renderer to use to render the scene.</param>
        public virtual void Render(GameRenderer renderer)
        {
            Entities.ForEach(e =>
            {
                renderer.Render(e);
                e.Render(renderer);
            });

            IsRenderingScene = false;
        }


        //TODO: Make this class IEnumarable so we get the benefits of generics and IList functionality
        //in the class itself.
        public void AddEntity(Entity entity, bool addToPhysics = true)
        {
            if(addToPhysics)
                PhysicsWorld.AddBody(entity.Body.InternalPhysicsBody);

            Entities.Add(entity);
        }
        #endregion
    }
}
