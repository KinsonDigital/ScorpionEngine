using KDScorpionCore;
using KDScorpionEngine.Graphics;
using KDScorpionEngine.Scene;

namespace KDScorpionEngineTests.Fakes
{
    public class FakeGameScene : GameScene
    {
        public FakeGameScene(Vector gravity) : base(gravity)
        {
        }


        public override void Render(GameRenderer renderer)
        {
            IsRenderingScene = true;

            base.Render(renderer);
        }
    }
}
