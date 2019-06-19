using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Entities;
using PluginSystem;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(assemblyName: "ScorpionEngineTests", AllInternalsVisible = true)]

namespace KDScorpionEngine.Graphics
{
    /// <summary>
    /// Renders entities to the screen.
    /// </summary>
    public class GameRenderer : Renderer
    {
        #region Fields
        private readonly IDebugDraw _debugDraw;
        #endregion

        internal string KinsonDigital { get; set;}

        #region Constructrons
        /// <summary>
        /// Creates a new instance of <see cref="GameRenderer"/>.
        /// </summary>
        /// <param name="renderer">The internal renderer implementation provided by a plugin.</param>
        public GameRenderer(IRenderer renderer, IDebugDraw debugDraw = null) : base(renderer) => _debugDraw = debugDraw;
        #endregion


        #region Public Methods
        /// <summary>
        /// Renders the given entity.
        /// </summary>
        /// <param name="entity">The entity to render.</param>
        public void Render(Entity entity)
        {
            Render(entity.Texture, entity.Position.X, entity.Position.Y, entity.Body.InternalPhysicsBody.Angle);

            //Render the physics bodies vertices to show its shape for debugging purposes
            if (entity.DebugDrawEnabled)
                _debugDraw.Draw(InternalRenderer, entity.Body.InternalPhysicsBody, entity.DebugDrawColor);
        }
        #endregion
    }
}
