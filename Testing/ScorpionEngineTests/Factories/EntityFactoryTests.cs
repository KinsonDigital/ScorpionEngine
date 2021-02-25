﻿// <copyright file="EntityFactoryTests.cs" company="KinsonDigital">
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
            Assert.Equal(TextureAtlasName, entity.RenderSection.TextureName);
            Assert.Equal("test-sub-texture", entity.RenderSection.SubTextureName);
            Assert.Null(entity.RenderSection.Animator);
            Assert.Null(entity.AtlasData);
            Assert.Equal(TextureType.SubTexture, entity.RenderSection.TypeOfTexture);
        }

        [Fact]
        public void CreateNonAnimatedFromTexture_WhenInvoked_CreatesEntityWithCorrectState()
        {
            // Act
            var entity = EntityFactory.CreateNonAnimatedFromTexture<Entity>(WholeTextureName);

            // Assert
            Assert.Equal(WholeTextureName, entity.RenderSection.TextureName);
            Assert.Equal(string.Empty, entity.RenderSection.SubTextureName);
            Assert.Null(entity.AtlasData);
            Assert.Null(entity.RenderSection.Animator);
            Assert.Equal(TextureType.WholeTexture, entity.RenderSection.TypeOfTexture);
        }

        [Fact]
        public void CreateAnimated_WhenInvoked_CreatesEntityWithCorrectState()
        {
            // Act
            var entity = EntityFactory.CreateAnimated<Entity>(TextureAtlasName, SubTextureName);

            // Assert
            Assert.Equal(TextureAtlasName, entity.RenderSection.TextureName);
            Assert.Equal(SubTextureName, entity.RenderSection.SubTextureName);
            Assert.Null(entity.AtlasData);
            Assert.NotNull(entity.RenderSection.Animator);
            Assert.Equal(TextureType.SubTexture, entity.RenderSection.TypeOfTexture);
        }

        [Fact]
        public void CreateAnimated_WhenInvokedWithAnimator_CreatesEntityWithCorrectState()
        {
            // Act
            var mockAnimator = new Mock<IAnimator>();

            var entity = EntityFactory.CreateAnimated<Entity>(TextureAtlasName, SubTextureName, mockAnimator.Object);

            // Assert
            Assert.Equal(TextureAtlasName, entity.RenderSection.TextureName);
            Assert.Equal(SubTextureName, entity.RenderSection.SubTextureName);
            Assert.Null(entity.AtlasData);
            Assert.NotNull(entity.RenderSection.Animator);
            Assert.Same(entity.RenderSection.Animator, mockAnimator.Object);
            Assert.Equal(TextureType.SubTexture, entity.RenderSection.TypeOfTexture);
        }
        #endregion
    }
}