using ScorpionCore.Graphics;

namespace ScorpionUI
{
    /// <summary>
    /// Gives the ability to render an object to the screen.
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// Renders the object to the screen.
        /// </summary>
        /// <param name="renderer">Renders the object the screen.</param>
        void Render(Renderer renderer);
    }
}
