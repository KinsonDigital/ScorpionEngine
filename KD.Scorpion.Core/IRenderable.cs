using KDScorpionCore.Graphics;

namespace KDScorpionCore
{
    /// <summary>
    /// Gives the ability to render an object to the screen.
    /// </summary>
    public interface IRenderable
    {
        #region Methods
        /// <summary>
        /// Renders the object to the screen.
        /// </summary>
        /// <param name="renderer">Renders the object the screen.</param>
        void Render(Renderer renderer);
        #endregion
    }
}
