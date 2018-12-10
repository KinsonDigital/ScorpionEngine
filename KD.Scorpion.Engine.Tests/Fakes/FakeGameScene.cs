using KDScorpionCore;
using KDScorpionCore.Graphics;
using KDScorpionEngine.Scene;

namespace KDScorpionEngine.Tests.Fakes
{
    public class FakeGameScene : GameScene
    {
        public FakeGameScene(Vector gravity) : base(gravity)
        {
        }


        public override void Render(Renderer renderer)
        {
            IsRenderingScene = true;

            base.Render(renderer);
        }
    }
}
