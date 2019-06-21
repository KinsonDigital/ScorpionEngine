using KDScorpionCore;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Graphics;
using KDScorpionEngine.Scene;

namespace KDScorpionEngineTests.Fakes
{
    public class FakeGameScene : GameScene
    {
        public FakeGameScene(Vector gravity, IPhysicsWorld physicsWorld) : base(gravity, physicsWorld)
        {
        }


        public override void Render(GameRenderer renderer)
        {
            IsRenderingScene = true;

            base.Render(renderer);
        }
    }
}
