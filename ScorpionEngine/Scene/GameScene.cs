using ScorpionCore;
using ScorpionCore.Plugins;
using ScorpionEngine.Content;
using ScorpionEngine.Graphics;
using ScorpionEngine.Entities;
using ScorpionEngine.Physics;
using System.Collections.Generic;

namespace ScorpionEngine.Scene
{
    public abstract class GameScene : IScene
    {
        public GameScene(Vector gravity)
        {
            PhysicsWorld = new PhysicsWorld(gravity);
        }


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
        public bool RenderingScene { get; set; }

        public List<Entity> Entities { get; } = new List<Entity>();

        public static PhysicsWorld PhysicsWorld { get; set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Initializes the game scene.
        /// </summary>
        public virtual void Initialize()
        {
            Initialized = true;
        }


        /// <summary>
        /// Loads all content for the scene using the given <see cref="ContentManager"/>.
        /// </summary>
        /// <param name="contentManager">The content manager to use for loading the scene's content.</param>
        public virtual void LoadContent(ContentLoader contentManager)
        {
            ContentLoaded = true;
        }


        /// <summary>
        /// Unloads all content for the scene.
        /// </summary>
        /// <param name="contentManager">The content manager to use for loading the scene's content.</param>
        public virtual void UnloadContent(ContentLoader contentManager)
        {
            ContentLoaded = false;
        }


        /// <summary>
        /// Updates the game object.
        /// </summary>
        public virtual void Update(EngineTime engineTime)
        {
            TimeManager?.Update(engineTime);

            //Update the physics world
            PhysicsWorld.Update((float)engineTime.ElapsedEngineTime.TotalSeconds);

            //Update all of the entities
            foreach (var entity in Entities)
            {
                entity.OnUpdate(engineTime);
            }
        }


        /// <summary>
        /// Renders the <see cref="GameScene"/>.
        /// </summary>
        /// <param name="renderer">The renderer to use for rendering.</param>
        public virtual void Render(Renderer renderer)
        {

        }


        public void AddEntity(Entity entity)
        {
            Entities.Add(entity);
        }
        #endregion
    }
}
