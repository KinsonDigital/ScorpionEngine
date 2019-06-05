namespace KDScorpionCore.Plugins
{
    /// <summary>
    /// Provides the ability to draw frames around a physics body shape for debugging purposes.
    /// </summary>
    public interface IDebugDraw : IPlugin
    {
        #region Methods
        /// <summary>
        /// Draws and outline around the given <paramref name="body"/> using the given <paramref name="renderer"/>.
        /// </summary>
        /// <param name="renderer">The renderer to use for rendering the outline/frame.</param>
        /// <param name="body">The body to render the outline/frame around.</param>
        void Draw(IRenderer renderer, IPhysicsBody body);
        #endregion
    }
}
