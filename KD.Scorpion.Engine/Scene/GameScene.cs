using KDScorpionCore;
using KDScorpionCore.Content;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Entities;
using KDScorpionEngine.Graphics;
using KDScorpionEngine.Physics;
using System.Collections.Generic;

namespace KDScorpionEngine.Scene
{
    public abstract class GameScene : IScene
    {
        #region Constructors
        internal GameScene(Vector gravity, IPhysicsWorld physicsWorld) => PhysicsWorld = new PhysicsWorld(gravity, physicsWorld);


        public GameScene(Vector gravity) => PhysicsWorld = new PhysicsWorld(gravity);
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the name of the scene.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating that the scene's content has been loaded.
        /// </summary>
        public bool ContentLoaded { get; set; }

        /// <summary>
        /// Gets or sets the time manager that manges the scene's frame timing and run mode.
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

        public List<Entity> Entities { get; } = new List<Entity>();

        public static PhysicsWorld PhysicsWorld { get; set; }

        public int Id { get; set; } = -1;
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
        /// Loads all content for the scene using the given <see cref="ContentManager"/>.
        /// </summary>
        /// <param name="contentLoader">The content manager to use for loading the scene's content.</param>
        public virtual void LoadContent(ContentLoader contentLoader)
        {
            ContentLoaded = true;
        }


        /// <summary>
        /// Unloads all content for the scene.
        /// </summary>
        /// <param name="contentLoader">The content manager to use for loading the scene's content.</param>
        public virtual void UnloadContent(ContentLoader contentLoader)
        {
            ContentLoaded = false;
        }


        /// <summary>
        /// Updates the game object.
        /// </summary>
        public virtual void Update(EngineTime engineTime)
        {
            TimeManager?.Update(engineTime);

            //Update all of the entities
            foreach (var entity in Entities)
            {
                entity.Update(engineTime);
            }

            //Update the physics world
            PhysicsWorld.Update((float)engineTime.ElapsedEngineTime.TotalSeconds);
        }


        /// <summary>
        /// Renders the <see cref="GameScene"/>.
        /// </summary>
        /// <param name="renderer">The renderer to use for rendering.</param>
        public virtual void Render(GameRenderer renderer)
        {
            foreach (var entity in Entities)
            {
                renderer.Render(entity);
                entity.Render(renderer);
            }

            IsRenderingScene = false;
        }


        //TODO: Make this class IEnumarable so we get the benefits of generics and IList functionality
        //in the class itself.
        public void AddEntity(Entity entity, bool addToPhysics = true)
        {
            if(addToPhysics)
                PhysicsWorld.AddEntity(entity);

            Entities.Add(entity);
        }
        #endregion
    }
}
