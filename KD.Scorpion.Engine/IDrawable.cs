using KDScorpionEngine.Graphics;

namespace KDScorpionEngine
{
    /// <summary>
    /// Provides functionality for game content to be rendered to the screen.
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Renders things to the screen.
        /// </summary>
        /// <param name="renderer">The rederer to use for rendering.</param>
        void Render(GameRenderer renderer);
    }
}
