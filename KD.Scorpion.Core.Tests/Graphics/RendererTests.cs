using Moq;
using NUnit.Framework;
using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using KDScorpionCore;

namespace KDScorpionCoreTests.Graphics
{
    public class RendererTests
    {
        #region Fields
        private Texture _texture;
        private GameText _gameText;
        #endregion


        #region Method Tests
        [Test]
        public void Render_WhenUsingTextureAndXAndY_InvokesInteralRenderMethod()
        {
            //Arrange
            var mockTexture = new Mock<ITexture>();
            var mockInternalRenderer = new Mock<IRenderer>();

            var renderer = new Renderer(mockInternalRenderer.Object);

            //Assert
            renderer.Render(_texture, It.IsAny<float>(), It.IsAny<float>());
            mockInternalRenderer.Verify(m => m.Render(_texture.InternalTexture, It.IsAny<float>(), It.IsAny<float>()), Times.Once());
        }


        [Test]
        public void Render_WhenUsingTextureAndXAndYAndAngle_InvokesInteralRenderMethod()
        {
            //Arrange
            var mockTexture = new Mock<ITexture>();
            var mockInternalRenderer = new Mock<IRenderer>();

            var renderer = new Renderer(mockInternalRenderer.Object);

            //Assert
            renderer.Render(_texture, It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>());
            mockInternalRenderer.Verify(m => m.Render(_texture.InternalTexture, It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()), Times.Once());
        }


        [Test]
        public void Render_WhenUsingTextureAndVector_InvokesInteralRenderMethod()
        {
            //Arrange
            var mockInternalRenderer = new Mock<IRenderer>();
           
            var renderer = new Renderer(mockInternalRenderer.Object);

            //Act
            renderer.Render(_texture, It.IsAny<Vector>());

            //Assert
            mockInternalRenderer.Verify(m => m.Render(It.IsAny<ITexture>(), It.IsAny<float>(), It.IsAny<float>()), Times.Once());
        }


        [Test]
        public void Render_WhenUsingGameTextAndXAndY_InternalRenderMethodInvoked()
        {
            //Arrange
            var mockInternalRenderer = new Mock<IRenderer>();
            var renderer = new Renderer(mockInternalRenderer.Object);

            //Assert
            renderer.Render(_gameText, It.IsAny<float>(), It.IsAny<float>());
            mockInternalRenderer.Verify(m => m.Render(_gameText.InternalText, It.IsAny<float>(), It.IsAny<float>()), Times.Once());
        }


        [Test]
        public void Render_WhenUsingGameTextAndXAndYAndGameColor_InternalRenderMethodInvoked()
        {
            //Arrange
            var mockText = new Mock<IText>();
            mockText.SetupGet(m => m.Color).Returns(new byte[] { 11, 22, 33, 44 });
            var mockInternalRenderer = new Mock<IRenderer>();

            var renderer = new Renderer(mockInternalRenderer.Object);
            var gameColor = It.IsAny<GameColor>();
            var expected = new GameColor(44, 11, 22, 33);

            //Act
            renderer.Render(_gameText, It.IsAny<float>(), It.IsAny<float>(), gameColor);
            var actual = _gameText.Color;

            //Assert
            mockInternalRenderer.Verify(m => m.Render(_gameText.InternalText, It.IsAny<float>(), It.IsAny<float>()), Times.Once());
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Start_WhenInvoking_InvokesInternalRendererStart()
        {
            //Arrange
            var mockInternalRenderer = new Mock<IRenderer>();
            var renderer = new Renderer(mockInternalRenderer.Object);

            //Act
            renderer.Start();

            //Assert
            mockInternalRenderer.Verify(m => m.Start(), Times.Once());
        }


        [Test]
        public void Stop_WhenInvoking_InvokesInternalRendererStart()
        {
            //Arrange
            var mockInternalRenderer = new Mock<IRenderer>();
            var renderer = new Renderer(mockInternalRenderer.Object);

            //Act
            renderer.End();

            //Assert
            mockInternalRenderer.Verify(m => m.End(), Times.Once());
        }


        [Test]
        public void Clear_WhenInvoking_InvokesInternalClear()
        {
            //Arrange
            var mockInternalRenderer = new Mock<IRenderer>();
            var renderer = new Renderer(mockInternalRenderer.Object);

            //Act
            renderer.Clear(It.IsAny<byte>(), It.IsAny<byte>(), It.IsAny<byte>(), It.IsAny<byte>());

            //Assert
            mockInternalRenderer.Verify(m => m.Clear(It.IsAny<byte>(), It.IsAny<byte>(), It.IsAny<byte>(), It.IsAny<byte>()), Times.Once());
        }


        [Test]
        public void FillCircle_WhenInvoking_InvokesInternalFillCircle()
        {
            //Arrange
            var mockInternalRenderer = new Mock<IRenderer>();
            var renderer = new Renderer(mockInternalRenderer.Object);

            //Act
            renderer.FillCircle(It.IsAny<Vector>(), It.IsAny<float>(), It.IsAny<GameColor>());

            //Assert
            mockInternalRenderer.Verify(m => m.FillCircle(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<byte[]>()), Times.Once());
        }
        #endregion


        #region Public Methods
        [SetUp]
        public void Setup()
        {
            var mockTexture = new Mock<ITexture>();

            _texture = new Texture(mockTexture.Object);

            var mockText = new Mock<IText>();
            mockText.SetupGet(m => m.Color).Returns(new byte[] { 11, 22, 33, 44 });
            _gameText = new GameText()
            {
                InternalText = mockText.Object
            };
        }


        [TearDown]
        public void TearDown()
        {
            _texture = null;
        }
        #endregion
    }
}
