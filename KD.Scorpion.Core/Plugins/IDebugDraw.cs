namespace KDScorpionCore.Plugins
{
    public interface IDebugDraw : IPlugin
    {
        void Draw(IRenderer renderer, IPhysicsBody body);
    }
}
