using ScorpionEngine.Content;

namespace ScorpionEngine.Scene
{
    /// <summary>
    /// Represents a single scene.
    /// </summary>
    public interface IScene : IDrawable, IUpdatable
    {
        #region Props
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
        /// Gets or sets a value indicating if the scene is currently rendering.
        /// </summary>
        bool RenderingScene { get; set; }
        #endregion


        #region Methods
        /// <summary>
        /// Initializes the scene.
        /// </summary>
        void Initialize();


        /// <summary>
        /// Loads all content for the scene using the given <see cref="ContentManager"/>.
        /// </summary>
        /// <param name="contentManager">The content manager to use for loading the scene's content.</param>
        void LoadContent(ContentLoader contentLoader);


        /// <summary>
        /// Unloads all content for the scene using the given <see cref="ContentManager"/>.
        /// </summary>
        void UnloadContent(ContentLoader contentLoader);
        #endregion
    }
}
