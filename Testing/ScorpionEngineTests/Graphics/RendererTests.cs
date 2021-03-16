// <copyright file="RendererTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Graphics
{
    using System.Drawing;
    using System.Numerics;
    using KDScorpionEngine.Entities;
    using KDScorpionEngine.Graphics;
    using Moq;
    using Raptor.Content;
    using Raptor.Graphics;
    using Xunit;

    /// <summary>
    /// Tests the <see cref="Renderer"/> class.
    /// </summary>
    public class RendererTests
    {
        private readonly Mock<ISpriteBatch> mockSpriteBatch;
        private readonly Mock<ITexture> mockTexture;

        /// <summary>
        /// Initializes a new instance of the <see cref="RendererTests"/> class.
        /// </summary>
        public RendererTests()
        {
            this.mockSpriteBatch = new Mock<ISpriteBatch>();
            this.mockTexture = new Mock<ITexture>();
            this.mockTexture.SetupGet(p => p.Width).Returns(300);
            this.mockTexture.SetupGet(p => p.Height).Returns(400);
        }

        #region Method Tests
        [Fact]
        public void Clear_WhenInvoked_ClearsSpriteBatch()
        {
            // Arrange
            var renderer = CreateRenderer();

            // Act
            renderer.Clear();

            // Assert
            this.mockSpriteBatch.Verify(m => m.Clear(), Times.Once());
        }

        [Fact]
        public void Begin_WhenInvoked_BeingsBatch()
        {
            // Arrange
            var renderer = CreateRenderer();

            // Act
            renderer.Begin();

            // Assert
            this.mockSpriteBatch.Verify(m => m.BeginBatch(), Times.Once());
        }

        [Fact]
        public void End_WhenInvoked_EndsBatch()
        {
            // Arrange
            var renderer = CreateRenderer();

            // Act
            renderer.End();

            // Assert
            this.mockSpriteBatch.Verify(m => m.EndBatch(), Times.Once());
        }

        [Fact]
        public void Render_WithHiddenEntity_DoesNotRenderEntity()
        {
            // Arrange
            var mockEntity = new Mock<IEntity>();
            mockEntity.SetupGet(p => p.Visible).Returns(false);
            mockEntity.SetupGet(p => p.Texture).Returns(this.mockTexture.Object);
            mockEntity.SetupGet(p => p.SectionToRender).Returns(new RenderSection());

            var renderer = CreateRenderer();

            // Act
            renderer.Render(mockEntity.Object);

            // Assert
            this.mockSpriteBatch.Verify(m => m.Render(
                It.IsAny<ITexture>(),
                It.IsAny<Rectangle>(),
                It.IsAny<Rectangle>(),
                It.IsAny<float>(),
                It.IsAny<float>(),
                It.IsAny<Color>()), Times.Never());
        }

        [Fact]
        public void Render_WithNullEntityTexture_DoesNotRenderEntity()
        {
            // Arrange
            var mockEntity = new Mock<IEntity>();
            mockEntity.SetupGet(p => p.Visible).Returns(true);
            mockEntity.SetupGet(p => p.SectionToRender).Returns(new RenderSection());

            var renderer = CreateRenderer();

            // Act
            renderer.Render(mockEntity.Object);

            // Assert
            this.mockSpriteBatch.Verify(m => m.Render(
                It.IsAny<ITexture>(),
                It.IsAny<Rectangle>(),
                It.IsAny<Rectangle>(),
                It.IsAny<float>(),
                It.IsAny<float>(),
                It.IsAny<Color>()), Times.Never());
        }

        [Fact]
        public void Render_WhenRenderingEntity_RendersEntity()
        {
            // Arrange
            var mockEntity = new Mock<IEntity>();
            mockEntity.SetupGet(p => p.Visible).Returns(true);
            mockEntity.SetupGet(p => p.Texture).Returns(this.mockTexture.Object);
            mockEntity.SetupGet(p => p.Position).Returns(new Vector2(100, 200));
            mockEntity.SetupGet(p => p.RenderSection).Returns(() =>
            mockEntity.SetupGet(p => p.SectionToRender).Returns(() =>
            {
                return new RenderSection()
                {
                    TypeOfTexture = TextureType.SubTexture,
                    RenderBounds = new Rectangle(11, 22, 33, 44),
                };
            });

            var renderer = CreateRenderer();
            var srcRect = new Rectangle(11, 22, 33, 44);
            var destRect = new Rectangle(100, 200, 300, 400);

            // Act
            renderer.Render(mockEntity.Object);

            // Assert
            this.mockSpriteBatch.Verify(m => m.Render(
                this.mockTexture.Object,
                srcRect,
                destRect,
                1,
                0,
                Color.White), Times.Once());
        }

        [Fact]
        public void Render_WhenRenderingTexture_RendersTexture()
        {
            // Arrange
            var renderer = CreateRenderer();

            // Act
            renderer.Render(this.mockTexture.Object, 100, 200);

            // Assert
            this.mockSpriteBatch.Verify(m => m.Render(this.mockTexture.Object, 100, 200));
        }

        [Fact]
        public void Dispose_WhenInvoked_DisposesOfSpriteBatch()
        {
            // Arrange
            var renderer = CreateRenderer();

            // Act
            renderer.Dispose();
            renderer.Dispose();

            // Assert
            this.mockSpriteBatch.Verify(m => m.Dispose(), Times.Once());
        }
        #endregion

        /// <summary>
        /// Creates a new instance of <see cref="Renderer"/> for the purpose of testing.
        /// </summary>
        /// <returns>The instance to test.</returns>
        private Renderer CreateRenderer() => new Renderer(this.mockSpriteBatch.Object, 100, 200);
    }
}
