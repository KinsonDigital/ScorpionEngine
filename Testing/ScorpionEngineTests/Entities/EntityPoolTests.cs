// <copyright file="EntityPoolTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Entities
{
    using System;
    using System.Drawing;
    using KDScorpionEngine;
    using KDScorpionEngine.Entities;
    using KDScorpionEngine.Factories;
    using KDScorpionEngine.Graphics;
    using KDScorpionEngineTests.Fakes;
    using Moq;
    using Raptor.Content;
    using Raptor.Graphics;
    using Xunit;

    /// <summary>
    /// Tests the <see cref="EntityPool"/> class.
    /// </summary>
    public class EntityPoolTests
    {
        private const string TextureAtlasName = "texture-atlas";
        private const string WholeTextureName = "whole-texture";
        private const string SubTextureName = "test-sub-texture";
        private readonly Mock<ITexture> mockAtlasTexture;
        private readonly Mock<ITexture> mockWholeTexture;
        private readonly Mock<IContentLoader> mockContentLoader;
        private readonly Mock<IEntityFactory> mockEntityFactory;
        private readonly Mock<IRenderer> mockRenderer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityPoolTests"/> class.
        /// </summary>
        public EntityPoolTests()
        {
            this.mockAtlasTexture = new Mock<ITexture>();
            this.mockWholeTexture = new Mock<ITexture>();

            this.mockContentLoader = new Mock<IContentLoader>();
            this.mockContentLoader.Setup(m => m.Load<IAtlasData>(TextureAtlasName))
                .Returns(() =>
                {
                    var atlasData = new AtlasSubTextureData[]
                    {
                        new AtlasSubTextureData()
                        {
                            Name = SubTextureName,
                            Bounds = new Rectangle(10, 20, 30, 40),
                            FrameIndex = -1,
                        },
                    };

                    return new AtlasData(atlasData, this.mockAtlasTexture.Object, TextureAtlasName, string.Empty);
                });
            this.mockContentLoader.Setup(m => m.Load<ITexture>(WholeTextureName))
                .Returns(this.mockWholeTexture.Object);

            this.mockEntityFactory = new Mock<IEntityFactory>();
            this.mockRenderer = new Mock<IRenderer>();
        }

        #region Prop Tests
        [Theory]
        [InlineData(true, true, 1)]
        [InlineData(true, false, 0)]
        [InlineData(false, true, 0)]
        [InlineData(false, false, 0)]
        public void TotalActiveItems_WhenGettingValue_ReturnsCorrectValue(bool visible, bool enabled, int expected)
        {
            // Arrange
            var entity = new FakeEntity();
            entity.Visible = visible;
            entity.Enabled = enabled;

            this.mockEntityFactory
                .Setup(m => m.CreateAnimated<FakeEntity>(TextureAtlasName, SubTextureName))
                .Returns(entity);

            var pool = CreatePool();

            // Act
            pool.GenerateAnimated(TextureAtlasName, SubTextureName);

            // Assert
            Assert.Equal(expected, pool.TotalActive);
        }

        [Theory]
        [InlineData(true, true, 0)]
        [InlineData(false, true, 0)]
        [InlineData(true, false, 0)]
        [InlineData(false, false, 1)]
        public void TotalInActiveItems_WhenGettingValue_ReturnsCorrectValue(bool visible, bool enabled, int expected)
        {
            // Arrange
            var entity = new FakeEntity();
            entity.Visible = visible;
            entity.Enabled = enabled;

            this.mockEntityFactory
                .Setup(m => m.CreateAnimated<FakeEntity>(TextureAtlasName, SubTextureName))
                .Returns(entity);

            var pool = CreatePool();

            // Act
            pool.GenerateAnimated(TextureAtlasName, SubTextureName);

            // Assert
            Assert.Equal(expected, pool.TotalInactive);
        }
        #endregion

        #region Method Tests
        [Fact]
        public void GenerateAnimated_WithItemToRecycle_WhenPoolSizeMaxIsReached_DoNotGenerateItem()
        {
            // Arrange
            var entity = new FakeEntity();

            this.mockEntityFactory
                .Setup(m => m.CreateAnimated<FakeEntity>(TextureAtlasName, SubTextureName))
                .Returns(entity);

            var pool = CreatePool();
            pool.MaxPoolSize = 1;
            pool.GenerateAnimated(TextureAtlasName, SubTextureName);

            // Act
            pool.GenerateAnimated(TextureAtlasName, SubTextureName);
            var actual = pool.Entitities[0];

            // Assert
            this.mockEntityFactory
                .Verify(m => m.CreateAnimated<FakeEntity>(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void GenerateAnimated_WithItemToRecycle_RecyclesItem()
        {
            // Arrange
            var entity = new FakeEntity();

            this.mockEntityFactory
                .Setup(m => m.CreateAnimated<FakeEntity>(TextureAtlasName, SubTextureName))
                .Returns(entity);

            var pool = CreatePool();
            pool.GenerateAnimated(TextureAtlasName, SubTextureName);

            // Set the entity as available to be recycled/reused
            entity.Visible = false;
            entity.Enabled = false;

            pool.GenerateAnimated(TextureAtlasName, SubTextureName);

            // Act
            var actual = pool.Entitities[0];

            // Assert
            Assert.Same(entity, actual);
            Assert.Equal(1, pool.TotalActive);
            Assert.True(entity.IsInitialized);
            Assert.True(entity.ContentLoaded);
        }

        [Fact]
        public void GenerateNonAnimatedFromTextureAtlas_WithNullGeneratedEntity_ThrowsException()
        {
            // Arrange
            FakeEntity nullEntity = null;

            this.mockEntityFactory
                .Setup(m => m.CreateNonAnimatedFromTextureAtlas<FakeEntity>(TextureAtlasName, SubTextureName))
                .Returns(nullEntity);

            var pool = CreatePool();

            // Act & Assert
            AssertHelpers.ThrowsWithMessage<NullReferenceException>(() =>
            {
                pool.GenerateNonAnimatedFromTextureAtlas(TextureAtlasName, SubTextureName, (_) => { });
            }, $"The generated entity from '{TextureAtlasName}' with sub texture '{SubTextureName}' cannot be null.");
        }

        [Fact]
        public void GenerateNonAnimatedFromTextureAtlas_With3ParamOverloadWhenPoolSizeMaxIsReached_DoNotGenerateItem()
        {
            // Arrange
            var entity = new FakeEntity();

            this.mockEntityFactory
                .Setup(m => m.CreateNonAnimatedFromTextureAtlas<FakeEntity>(TextureAtlasName, SubTextureName))
                .Returns(entity);

            var pool = CreatePool();
            pool.MaxPoolSize = 1;

            pool.GenerateNonAnimatedFromTextureAtlas(TextureAtlasName, SubTextureName, (_) => { });

            // Act
            pool.GenerateNonAnimatedFromTextureAtlas(TextureAtlasName, SubTextureName, (_) => { });
            var actual = pool.Entitities[0];

            // Assert
            this.mockEntityFactory
                .Verify(m => m.CreateNonAnimatedFromTextureAtlas<FakeEntity>(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void GenerateNonAnimatedFromTextureAtlas_With3ParamOverloadWithItemToRecycle_RecyclesItem()
        {
            // Arrange
            var entity = new FakeEntity();

            this.mockEntityFactory
                .Setup(m => m.CreateNonAnimatedFromTextureAtlas<FakeEntity>(TextureAtlasName, SubTextureName))
                .Returns(entity);

            var pool = CreatePool();
            pool.GenerateNonAnimatedFromTextureAtlas(TextureAtlasName, SubTextureName, (_) => { });

            // Set the entity as available to be recycled/reused
            entity.Visible = false;
            entity.Enabled = false;

            // Act
            pool.GenerateNonAnimatedFromTextureAtlas(TextureAtlasName, SubTextureName, (_) => { });
            var actual = pool.Entitities[0];

            // Assert
            Assert.Same(entity, actual);
            Assert.True(entity.IsInitialized);
            Assert.True(entity.ContentLoaded);
        }

        [Fact]
        public void GenerateNonAnimatedFromTextureAtlas_With2ParamOverloadInvoked_GeneratesItem()
        {
            // Arrange
            var entity = new FakeEntity();
            this.mockEntityFactory
                .Setup(m => m.CreateNonAnimatedFromTextureAtlas<FakeEntity>(TextureAtlasName, SubTextureName))
                .Returns(entity);

            var pool = CreatePool();

            // Act
            pool.GenerateNonAnimatedFromTextureAtlas(TextureAtlasName, SubTextureName);

            // Assert
            this.mockEntityFactory
                .Verify(m => m.CreateNonAnimatedFromTextureAtlas<FakeEntity>(TextureAtlasName, SubTextureName), Times.Once());
        }

        [Fact]
        public void GenerateNonAnimatedFromTextureAtlas_With2ParamOverloadWithItemToRecycle_RecyclesItem()
        {
            // Arrange
            var entity = new FakeEntity();

            this.mockEntityFactory
                .Setup(m => m.CreateNonAnimatedFromTextureAtlas<FakeEntity>(TextureAtlasName, SubTextureName))
                .Returns(entity);

            var pool = CreatePool();
            pool.MaxPoolSize = 1;

            // Act
            pool.GenerateNonAnimatedFromTextureAtlas(TextureAtlasName, SubTextureName);
            pool.GenerateNonAnimatedFromTextureAtlas(TextureAtlasName, SubTextureName);

            // Assert
            this.mockEntityFactory
                .Verify(m => m.CreateNonAnimatedFromTextureAtlas<FakeEntity>(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void GenerateNonAnimatedFromTexture_WhenPoolSizeMaxIsReached_DoNotGenerateItem()
        {
            // Arrange
            var entity = new FakeEntity();

            this.mockEntityFactory
                .Setup(m => m.CreateNonAnimatedFromTexture<FakeEntity>(WholeTextureName))
                .Returns(entity);

            var pool = CreatePool();
            pool.MaxPoolSize = 1;

            // Act
            pool.GenerateNonAnimatedFromTexture(WholeTextureName);
            pool.GenerateNonAnimatedFromTexture(WholeTextureName);

            // Assert
            this.mockEntityFactory
                .Verify(m => m.CreateNonAnimatedFromTexture<FakeEntity>(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void GenerateNonAnimatedFromTexture_WithItemToRecycle_RecyclesItem()
        {
            // Arrange
            var entity = new FakeEntity();

            this.mockEntityFactory
                .Setup(m => m.CreateNonAnimatedFromTexture<FakeEntity>(WholeTextureName))
                .Returns(entity);

            var pool = CreatePool();
            pool.GenerateNonAnimatedFromTexture(WholeTextureName);

            // Set the entity as available to be recycled/reused
            entity.Visible = false;
            entity.Enabled = false;

            // Act
            pool.GenerateNonAnimatedFromTexture(WholeTextureName);
            var actual = pool.Entitities[0];

            // Assert
            Assert.Same(entity, actual);
            Assert.True(entity.IsInitialized);
            Assert.True(entity.ContentLoaded);
        }

        [Fact]
        public void Update_WhenInvoked_UpdatesEntities()
        {
            // Arrange
            var entity = new FakeEntity();

            this.mockEntityFactory
                .Setup(m => m.CreateNonAnimatedFromTexture<FakeEntity>(WholeTextureName))
                .Returns(entity);

            var pool = CreatePool();
            pool.GenerateNonAnimatedFromTexture(WholeTextureName);

            // Act
            pool.Update(new GameTime());

            // Assert
            Assert.True(entity.UpdateInvoked);
        }

        [Fact]
        public void Render_WhenInvoked_RendersEntities()
        {
            // Arrange
            var entity = new FakeEntity();

            this.mockEntityFactory
                .Setup(m => m.CreateNonAnimatedFromTexture<FakeEntity>(WholeTextureName))
                .Returns(entity);

            var pool = CreatePool();
            pool.GenerateNonAnimatedFromTexture(WholeTextureName);

            // Act
            pool.Render(this.mockRenderer.Object);

            // Assert
            this.mockRenderer.Verify(m => m.Render(entity), Times.Once());
        }
        #endregion

        /// <summary>
        /// Creates a <see cref="EntityPool{TEntity}"/> instance for the purpose of testing.
        /// </summary>
        /// <returns>The instance to test.</returns>
        private EntityPool<FakeEntity> CreatePool() => new EntityPool<FakeEntity>(this.mockContentLoader.Object, this.mockEntityFactory.Object);
    }
}
