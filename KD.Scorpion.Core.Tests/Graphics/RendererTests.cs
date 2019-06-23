using Moq;
using Xunit;
using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using KDScorpionCore;
using System;

namespace KDScorpionCoreTests.Graphics
{
    public class RendererTests : IDisposable
    {
        #region Private Fields
        private Texture _texture;
        private Mock<IDebugDraw> _debugDraw;
        private GameText _gameText;
        #endregion


        #region Constructors
        public RendererTests()
        {
            var mockTexture = new Mock<ITexture>();

            _texture = new Texture(mockTexture.Object);

            var mockText = new Mock<IText>();
            mockText.SetupGet(m => m.Color).Returns(new GameColor(11, 22, 33, 44));

            _debugDraw = new Mock<IDebugDraw>();

            _gameText = new GameText()
            {
                InternalText = mockText.Object
            };
        }
        #endregion


        #region Method Tests
        [Fact]
        public void Render_WhenUsingTextureAndXAndY_InvokesInteralRenderMethod()
        {
            //Arrange
            var mockTexture = new Mock<ITexture>();
            var mockRenderer = new Mock<IRenderer>();

            var renderer = new Renderer(mockRenderer.Object, _debugDraw.Object);

            //Assert
            renderer.Render(_texture, It.IsAny<float>(), It.IsAny<float>());
            mockRenderer.Verify(m => m.Render(_texture.InternalTexture, It.IsAny<float>(), It.IsAny<float>()), Times.Once());
        }


        [Fact]
        public void Render_WhenUsingTextureAndXAndYAndAngle_InvokesInteralRenderMethod()
        {
            //Arrange
            var mockTexture = new Mock<ITexture>();
            var mockInternalRenderer = new Mock<IRenderer>();

            var renderer = new Renderer(mockInternalRenderer.Object, _debugDraw.Object);

            //Assert
            renderer.Render(_texture, It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>());
            mockInternalRenderer.Verify(m => m.Render(_texture.InternalTexture, It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()), Times.Once());
        }


        [Fact]
        public void Render_WhenUsingTextureAndVector_InvokesInteralRenderMethod()
        {
            //Arrange
            var mockInternalRenderer = new Mock<IRenderer>();
           
            var renderer = new Renderer(mockInternalRenderer.Object, _debugDraw.Object);

            //Act
            renderer.Render(_texture, It.IsAny<Vector>());

            //Assert
            mockInternalRenderer.Verify(m => m.Render(It.IsAny<ITexture>(), It.IsAny<float>(), It.IsAny<float>()), Times.Once());
        }


        [Fact]
        public void Render_WhenUsingGameTextAndXAndY_InternalRenderMethodInvoked()
        {
            //Arrange
            var mockInternalRenderer = new Mock<IRenderer>();
            var renderer = new Renderer(mockInternalRenderer.Object, _debugDraw.Object);

            //Assert
            renderer.Render(_gameText, It.IsAny<float>(), It.IsAny<float>());
            mockInternalRenderer.Verify(m => m.Render(_gameText.InternalText, It.IsAny<float>(), It.IsAny<float>()), Times.Once());
        }


        [Fact]
        public void Render_WhenUsingGameTextAndXAndYAndGameColor_InternalRenderMethodInvoked()
        {
            //Arrange
            var mockInternalRenderer = new Mock<IRenderer>();

            var renderer = new Renderer(mockInternalRenderer.Object, _debugDraw.Object);

            //Act
            renderer.Render(_gameText, It.IsAny<float>(), It.IsAny<float>(), It.IsAny<GameColor>());

            //Assert
            mockInternalRenderer.Verify(m => m.Render(_gameText.InternalText, It.IsAny<float>(), It.IsAny<float>(), It.IsAny<GameColor>()), Times.Once());
        }


        [Fact]
        public void Start_WhenInvoking_InvokesInternalRendererStart()
        {
            //Arrange
            var mockInternalRenderer = new Mock<IRenderer>();
            var renderer = new Renderer(mockInternalRenderer.Object, _debugDraw.Object);

            //Act
            renderer.Start();

            //Assert
            mockInternalRenderer.Verify(m => m.Start(), Times.Once());
        }


        [Fact]
        public void Stop_WhenInvoking_InvokesInternalRendererStart()
        {
            //Arrange
            var mockInternalRenderer = new Mock<IRenderer>();
            var renderer = new Renderer(mockInternalRenderer.Object, _debugDraw.Object);

            //Act
            renderer.End();

            //Assert
            mockInternalRenderer.Verify(m => m.End(), Times.Once());
        }


        [Fact]
        public void Clear_WhenInvoking_InvokesInternalClear()
        {
            //Arrange
            var mockInternalRenderer = new Mock<IRenderer>();
            var renderer = new Renderer(mockInternalRenderer.Object, _debugDraw.Object);

            //Act
            renderer.Clear(It.IsAny<byte>(), It.IsAny<byte>(), It.IsAny<byte>(), It.IsAny<byte>());

            //Assert
            mockInternalRenderer.Verify(m => m.Clear(It.IsAny<GameColor>()), Times.Once());
        }


        [Fact]
        public void FillCircle_WhenInvoking_InvokesInternalFillCircle()
        {
            //Arrange
            var mockInternalRenderer = new Mock<IRenderer>();
            var renderer = new Renderer(mockInternalRenderer.Object, _debugDraw.Object);

            //Act
            renderer.FillCircle(It.IsAny<Vector>(), It.IsAny<float>(), It.IsAny<GameColor>());

            //Assert
            mockInternalRenderer.Verify(m => m.FillCircle(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<GameColor>()), Times.Once());
        }
        #endregion


        #region Public Methods
        public void Dispose() => _texture = null;
        #endregion
    }
}
