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
        public void Render_WithOnlyEntityParam_RendersEntity()
        {
            // Arrange
            var sectionToRender = new RenderSection()
            {
                RenderBounds = new Rectangle(11, 22, 33, 44),
                Animator = null,
                TypeOfTexture = TextureType.SubTexture,
            };

            var mockEntity = new Mock<IEntity>();
            mockEntity.SetupGet(p => p.Visible).Returns(true);
            mockEntity.SetupGet(p => p.Texture).Returns(this.mockTexture.Object);
            mockEntity.SetupGet(p => p.SectionToRender).Returns(sectionToRender);
            mockEntity.SetupGet(p => p.Position).Returns(new Vector2(55, 66));

            var renderer = CreateRenderer();

            // Act
            renderer.Render(mockEntity.Object);

            // Assert
            this.mockSpriteBatch.Verify(m => m.Render(
                this.mockTexture.Object,
                new Rectangle(11, 22, 33, 44),
                new Rectangle(55, 66, 300, 400),
                1,
                0,
                Color.White,
                RenderEffects.None), Times.Once());
        }

        [Fact]
        public void Render_WithEntityAndXAndYParams_RendersEntity()
        {
            // Arrange
            var sectionToRender = new RenderSection()
            {
                RenderBounds = new Rectangle(11, 22, 33, 44),
                Animator = null,
                TypeOfTexture = TextureType.SubTexture,
            };

            var mockEntity = new Mock<IEntity>();
            mockEntity.SetupGet(p => p.Visible).Returns(true);
            mockEntity.SetupGet(p => p.Texture).Returns(this.mockTexture.Object);
            mockEntity.SetupGet(p => p.SectionToRender).Returns(sectionToRender);

            var renderer = CreateRenderer();

            // Act
            renderer.Render(mockEntity.Object, 55, 66);

            // Assert
            this.mockSpriteBatch.Verify(m => m.Render(
                this.mockTexture.Object,
                new Rectangle(11, 22, 33, 44),
                new Rectangle(55, 66, 300, 400),
                1,
                0,
                Color.White,
                RenderEffects.None), Times.Once());
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
                It.IsAny<Color>(),
                It.IsAny<RenderEffects>()), Times.Never());
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
                It.IsAny<Color>(),
                It.IsAny<RenderEffects>()), Times.Never());
        }

        [Theory]
        [InlineData(false, false, RenderEffects.None)]
        [InlineData(true, false, RenderEffects.FlipHorizontally)]
        [InlineData(false, true, RenderEffects.FlipVertically)]
        public void Render_WhenRenderingEntity_RendersEntity(bool flippedHorizontally, bool flippedVertically, RenderEffects expected)
        {
            // Arrange
            var mockEntity = new Mock<IEntity>();
            mockEntity.SetupGet(p => p.Visible).Returns(true);
            mockEntity.SetupGet(p => p.Texture).Returns(this.mockTexture.Object);
            mockEntity.SetupGet(p => p.FlippedHorizontally).Returns(flippedHorizontally);
            mockEntity.SetupGet(p => p.FlippedVertically).Returns(flippedVertically);
            mockEntity.SetupGet(p => p.SectionToRender).Returns(() =>
            {
                return new RenderSection()
                {
                    TypeOfTexture = TextureType.SubTexture,
                    RenderBounds = new Rectangle(11, 22, 33, 44),
                };
            });

            var renderer = CreateRenderer();

            // Act
            renderer.Render(mockEntity.Object);

            // Assert
            this.mockSpriteBatch.Verify(m => m.Render(
                this.mockTexture.Object,
                It.IsAny<Rectangle>(),
                It.IsAny<Rectangle>(),
                It.IsAny<float>(),
                It.IsAny<float>(),
                It.IsAny<Color>(),
                expected), Times.Once());
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
