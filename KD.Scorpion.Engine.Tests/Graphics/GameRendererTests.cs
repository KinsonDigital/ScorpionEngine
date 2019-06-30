using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Graphics;
using KDScorpionEngineTests.Fakes;
using Moq;
using Xunit;

namespace KDScorpionEngineTests.Graphics
{
    public class GameRendererTests
    {
        [Fact]
        public void Renderer_WhenInvoked_InvokesInternalRenderer()
        {
            //Arrange
            var mockRenderer = new Mock<IRenderer>();
            var renderer = new GameRenderer(mockRenderer.Object, new Mock<IDebugDraw>().Object);
            var fakeEntity = new FakeEntity(new Mock<IPhysicsBody>().Object);
            var texture = new Texture(new Mock<ITexture>().Object);

            fakeEntity.Texture = texture;

            //Act
            renderer.Render(fakeEntity);

            //Assert
            mockRenderer.Verify(m => m.Render(It.IsAny<ITexture>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()));
        }
    }
}
