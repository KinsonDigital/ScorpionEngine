// <copyright file="EntityPoolTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Entities
{
    using System.Drawing;
    using KDScorpionEngine;
    using KDScorpionEngine.Entities;
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
        private readonly Mock<ISpriteBatch> mockSpriteBatch;
        private readonly Renderer renderer;

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


            this.mockSpriteBatch = new Mock<ISpriteBatch>();
            this.renderer = new Renderer(this.mockSpriteBatch.Object, 100, 100);
        }

        #region Prop Tests
        [Fact]
        public void TotalActiveItems_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var pool = CreatePool();

            // Act
            pool.GenerateAnimated(TextureAtlasName, SubTextureName);

            // Assert
            Assert.Equal(1, pool.TotalActive);
        }

        [Fact]
        public void TotalInActiveItems_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var pool = CreatePool();
            pool.GenerateAnimated(TextureAtlasName, SubTextureName);
            var entity = pool.Entitities[0];

            // Act
            pool.GenerateAnimated(TextureAtlasName, SubTextureName);
            entity.Visible = false;
            entity.Enabled = false;

            // Assert
            Assert.Equal(1, pool.TotalInactive);
        }
        #endregion

        #region Method Tests

        [Fact]
        public void GenerateAnimated_WithItemToRecycle_WhenPoolSizeMaxIsReached_DoNotGenerateItem()
        {
            // Arrange
            var pool = CreatePool();
            pool.MaxPoolSize = 1;

            pool.GenerateAnimated(TextureAtlasName, SubTextureName);
            var entity = pool.Entitities[0];

            // Act
            pool.GenerateAnimated(TextureAtlasName, SubTextureName);
            var actual = pool.Entitities[0];

            // Assert
            Assert.Same(entity, actual);
            Assert.Single(pool.Entitities);
        }

        [Fact]
        public void GenerateAnimated_WithItemToRecycle_RecyclesItem()
        {
            // Arrange
            var pool = CreatePool();
            pool.GenerateAnimated(TextureAtlasName, SubTextureName);
            var entity = pool.Entitities[0];

            // Set the entity as available to be recycled/reused
            entity.Visible = false;
            entity.Enabled = false;

            pool.GenerateAnimated(TextureAtlasName, SubTextureName);

            // Act
            var actual = pool.Entitities[0];

            // Assert
            Assert.Same(entity, actual);
            Assert.Equal(1, pool.TotalActive);
            Assert.True(entity.InitInvoked);
            Assert.True(entity.LoadContentInvoked);
        }

        [Fact]
        public void GenerateNonAnimatedFromTextureAtlas_WhenPoolSizeMaxIsReached_DoNotGenerateItem()
        {
            // Arrange
            var pool = CreatePool();
            pool.MaxPoolSize = 1;

            pool.GenerateNonAnimatedFromTextureAtlas(TextureAtlasName, SubTextureName);
            var entity = pool.Entitities[0];

            // Act
            pool.GenerateNonAnimatedFromTextureAtlas(TextureAtlasName, SubTextureName);
            var actual = pool.Entitities[0];

            // Assert
            Assert.Same(entity, actual);
            Assert.Single(pool.Entitities);
        }

        [Fact]
        public void GenerateNonAnimatedFromTextureAtlas_WithItemToRecycle_RecyclesItem()
        {
            // Arrange
            var pool = CreatePool();
            pool.GenerateNonAnimatedFromTextureAtlas(TextureAtlasName, SubTextureName);
            var entity = pool.Entitities[0];

            // Set the entity as available to be recycled/reused
            entity.Visible = false;
            entity.Enabled = false;

            // Act
            pool.GenerateNonAnimatedFromTextureAtlas(TextureAtlasName, SubTextureName);
            var actual = pool.Entitities[0];

            // Assert
            Assert.Same(entity, actual);
            Assert.True(entity.InitInvoked);
            Assert.True(entity.LoadContentInvoked);
        }

        [Fact]
        public void GenerateNonAnimatedFromTexture_WhenPoolSizeMaxIsReached_DoNotGenerateItem()
        {
            // Arrange
            var pool = CreatePool();
            pool.MaxPoolSize = 1;

            pool.GenerateNonAnimatedFromTexture(WholeTextureName);
            var entity = pool.Entitities[0];

            // Act
            pool.GenerateNonAnimatedFromTexture(WholeTextureName);
            var actual = pool.Entitities[0];

            // Assert
            Assert.Same(entity, actual);
            Assert.Single(pool.Entitities);
        }

        [Fact]
        public void GenerateNonAnimatedFromTexture_WithItemToRecycle_RecyclesItem()
        {
            // Arrange
            var pool = CreatePool();
            pool.GenerateNonAnimatedFromTexture(WholeTextureName);
            var entity = pool.Entitities[0];

            // Set the entity as available to be recycled/reused
            entity.Visible = false;
            entity.Enabled = false;

            // Act
            pool.GenerateNonAnimatedFromTexture(WholeTextureName);
            var actual = pool.Entitities[0];

            // Assert
            Assert.Same(entity, actual);
            Assert.True(entity.InitInvoked);
            Assert.True(entity.LoadContentInvoked);
        }

        [Fact]
        public void Update_WhenInvoked_UpdatesEntities()
        {
            // Arrange
            var pool = CreatePool();
            pool.GenerateNonAnimatedFromTexture(WholeTextureName);
            var entity = pool.Entitities[0];

            // Act
            pool.Update(new GameTime());

            // Assert
            Assert.True(entity.UpdateInvoked);
        }

        [Fact]
        public void Render_WhenInvoked_RendersEntities()
        {
            // Arrange
            var pool = CreatePool();
            pool.GenerateNonAnimatedFromTexture(WholeTextureName);
            var entity = pool.Entitities[0];

            // Act
            pool.Render(this.renderer);

            // Assert
            this.mockSpriteBatch.Verify(m => m.Render(It.IsAny<ITexture>(),
                                                      It.IsAny<Rectangle>(),
                                                      It.IsAny<Rectangle>(),
                                                      It.IsAny<float>(),
                                                      It.IsAny<float>(),
                                                      It.IsAny<Color>()));
        }
        #endregion

        /// <summary>
        /// Creates a <see cref="EntityPool{TEntity}"/> instance for the purpose of testing.
        /// </summary>
        /// <returns>The instance to test.</returns>
        private EntityPool<FakeEntity> CreatePool() => new EntityPool<FakeEntity>(this.mockContentLoader.Object);
    }
}
