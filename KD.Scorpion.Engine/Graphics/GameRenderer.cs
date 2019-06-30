using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Entities;
using System.Diagnostics.CodeAnalysis;

namespace KDScorpionEngine.Graphics
{
    /// <summary>
    /// Renders entities to the screen.
    /// </summary>
    public class GameRenderer : Renderer
    {
        #region Constructors
        internal GameRenderer(IRenderer renderer, IDebugDraw debugDraw) : base(renderer, debugDraw) { }

        /// <summary>
        /// Creates a new instance of <see cref="GameRenderer"/>.
        /// </summary>
        /// <param name="renderer">The internal renderer implementation provided by a plugin.</param>
        [ExcludeFromCodeCoverage]
        public GameRenderer() { }
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
                RenderDebugDraw(entity.Body.InternalPhysicsBody, new GameColor(255, 255, 255, 255));
        }
        #endregion
    }
}
