// <copyright file="EntityFactoryTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Factories
{
    using KDScorpionEngine.Entities;
    using KDScorpionEngine.Factories;
    using KDScorpionEngine.Graphics;
    using Moq;
    using Raptor.Content;
    using Xunit;

    /// <summary>
    /// Tests the <see cref="EntityFactory"/> class.
    /// </summary>
    public class EntityFactoryTests
    {
        private const string TextureAtlasName = "test-atlas";
        private const string WholeTextureName = "test-whole-texture";
        private const string SubTextureName = "test-sub-texture";

        #region Method Tests
        [Fact]
        public void CreateNonAnimatedFromTextureAtlas_WhenInvoked_CreatesEntityWithCorrectState()
        {
            // Act
            var entity = EntityFactory.CreateNonAnimatedFromTextureAtlas<Entity>(TextureAtlasName, SubTextureName);

            // Assert
            Assert.Equal(TextureAtlasName, entity.SectionToRender.TextureName);
            Assert.Equal("test-sub-texture", entity.SectionToRender.SubTextureName);
            Assert.Null(entity.SectionToRender.Animator);
            Assert.Null(entity.AtlasData);
            Assert.Equal(TextureType.SubTexture, entity.SectionToRender.TypeOfTexture);
        }

        [Fact]
        public void CreateNonAnimatedFromTexture_WhenInvoked_CreatesEntityWithCorrectState()
        {
            // Act
            var entity = EntityFactory.CreateNonAnimatedFromTexture<Entity>(WholeTextureName);

            // Assert
            Assert.Equal(WholeTextureName, entity.SectionToRender.TextureName);
            Assert.Equal(string.Empty, entity.SectionToRender.SubTextureName);
            Assert.Null(entity.AtlasData);
            Assert.Null(entity.SectionToRender.Animator);
            Assert.Equal(TextureType.WholeTexture, entity.SectionToRender.TypeOfTexture);
        }

        [Fact]
        public void CreateAnimated_WhenInvoked_CreatesEntityWithCorrectState()
        {
            // Act
            var entity = EntityFactory.CreateAnimated<Entity>(TextureAtlasName, SubTextureName);

            // Assert
            Assert.Equal(TextureAtlasName, entity.SectionToRender.TextureName);
            Assert.Equal(SubTextureName, entity.SectionToRender.SubTextureName);
            Assert.Null(entity.AtlasData);
            Assert.NotNull(entity.SectionToRender.Animator);
            Assert.Equal(TextureType.SubTexture, entity.SectionToRender.TypeOfTexture);
        }

        [Fact]
        public void CreateAnimated_WhenInvokedWithAnimator_CreatesEntityWithCorrectState()
        {
            // Act
            var mockAnimator = new Mock<IAnimator>();

            var entity = EntityFactory.CreateAnimated<Entity>(TextureAtlasName, SubTextureName, mockAnimator.Object);

            // Assert
            Assert.Equal(TextureAtlasName, entity.SectionToRender.TextureName);
            Assert.Equal(SubTextureName, entity.SectionToRender.SubTextureName);
            Assert.Null(entity.AtlasData);
            Assert.NotNull(entity.SectionToRender.Animator);
            Assert.Same(entity.SectionToRender.Animator, mockAnimator.Object);
            Assert.Equal(TextureType.SubTexture, entity.SectionToRender.TypeOfTexture);
        }
        #endregion
    }
}
