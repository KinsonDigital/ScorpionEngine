using KDScorpionCore.Plugins;
using KDScorpionEngine.Graphics;
using KDScorpionEngine.Scene;

namespace KDScorpionEngineTests.Fakes
{
    public class FakeGameScene : GameScene
    {
        public FakeGameScene(IPhysicsWorld physicsWorld) : base(physicsWorld)
        {
        }


        public override void Render(GameRenderer renderer)
        {
            IsRenderingScene = true;

            base.Render(renderer);
        }
    }
}
