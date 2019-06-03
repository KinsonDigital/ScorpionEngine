using KDScorpionCore.Plugins;

namespace PluginSystem
{
    public interface IPluginFactory
    {
        #region Methods
        IRenderer CreateRenderer();


        IContentLoader CreateContentLoader();


        IEngineCore CreateEngineCore();


        IKeyboard CreateKeyboard();


        IMouse CreateMouse();


        IDebugDraw CreateDebugDraw();


        IPhysicsBody CreatePhysicsBody();


        IPhysicsBody CreatePhysicsBody(params object[] paramItems);


        IPhysicsWorld CreatePhysicsWorld();


        IPhysicsWorld CreatePhysicsWorld(params object[] paramItems);
        #endregion
    }
}
