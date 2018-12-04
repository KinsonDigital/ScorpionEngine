﻿using Moq;
using NUnit.Framework;
using ScorpionCore.Graphics;
using ScorpionCore.Plugins;

namespace ScorpionCore.Tests.Graphics
{
    public class RendererTests
    {
        #region Method Tests
        [Test]
        public void Render_WhenUsingTextureAndXAndY_InvokesInteralRenderMethod()
        {
            //Arrange
            var mockTexture = new Mock<ITexture>();
            var mockInternalRenderer = new Mock<IRenderer>();

            var renderer = new Renderer(mockInternalRenderer.Object);
            var texture = new Texture()
            {
                InternalTexture = mockTexture.Object
            };

            //Assert
            renderer.Render(texture, It.IsAny<float>(), It.IsAny<float>());
            mockInternalRenderer.Verify(m => m.Render(texture.InternalTexture, It.IsAny<float>(), It.IsAny<float>()), Times.Once());
        }


        [Test]
        public void Render_WhenUsingTextureAndXAndYAndAngle_InvokesInteralRenderMethod()
        {
            //Arrange
            var mockTexture = new Mock<ITexture>();
            var mockInternalRenderer = new Mock<IRenderer>();

            var renderer = new Renderer(mockInternalRenderer.Object);
            var texture = new Texture()
            {
                InternalTexture = mockTexture.Object
            };

            //Assert
            renderer.Render(texture, It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>());
            mockInternalRenderer.Verify(m => m.Render(texture.InternalTexture, It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()), Times.Once());
        }


        [Test]
        public void Render_WhenUsingTextureAndVector_InvokesInteralRenderMethod()
        {
            //Arrange
            var mockInternalRenderer = new Mock<IRenderer>();

            var renderer = new Renderer(mockInternalRenderer.Object);

            //Act
            renderer.Render(It.IsAny<Texture>(), It.IsAny<Vector>());

            //Assert
            mockInternalRenderer.Verify(m => m.Render(It.IsAny<ITexture>(), It.IsAny<float>(), It.IsAny<float>()), Times.Once());
        }


        [Test]
        public void Render_WhenUsingGameTextAndXAndY_InternalRenderMethodInvoked()
        {
            //Arrange
            var mockText = new Mock<IText>();
            var mockInternalRenderer = new Mock<IRenderer>();

            var renderer = new Renderer(mockInternalRenderer.Object);
            var gameText = new GameText()
            {
                InternalText = mockText.Object
            };

            //Assert
            renderer.Render(gameText, It.IsAny<float>(), It.IsAny<float>());
            mockInternalRenderer.Verify(m => m.Render(gameText.InternalText, It.IsAny<float>(), It.IsAny<float>()), Times.Once());
        }


        [Test]
        public void Render_WhenUsingGameTextAndXAndYAndGameColor_InternalRenderMethodInvoked()
        {
            //Arrange
            var mockText = new Mock<IText>();
            mockText.SetupGet(m => m.Color).Returns(new byte[] { 11, 22, 33, 44 });
            var mockInternalRenderer = new Mock<IRenderer>();

            var renderer = new Renderer(mockInternalRenderer.Object);
            var gameText = new GameText()
            {
                InternalText = mockText.Object
            };
            var gameColor = It.IsAny<GameColor>();
            var expectedColor = new GameColor(11, 22, 33, 44);

            //Act
            renderer.Render(gameText, It.IsAny<float>(), It.IsAny<float>(), gameColor);
            var actualColor = gameText.Color;

            //Assert
            mockInternalRenderer.Verify(m => m.Render(gameText.InternalText, It.IsAny<float>(), It.IsAny<float>()), Times.Once());
            Assert.AreEqual(expectedColor, actualColor);
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
            var red = It.IsAny<byte>();
            var green = It.IsAny<byte>();
            var blue = It.IsAny<byte>();
            var alpha = It.IsAny<byte>();

            //Act
            renderer.Clear(red, green, blue, alpha);

            //Assert
            mockInternalRenderer.Verify(m => m.Clear(red, green, blue, alpha), Times.Once());
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
    }
}
