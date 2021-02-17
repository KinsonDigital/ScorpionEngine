// <copyright file="EntityTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Entities
{
    using System;
    using System.Drawing;
    using System.Numerics;
    using KDScorpionEngine;
    using KDScorpionEngine.Behaviors;
    using KDScorpionEngine.Entities;
    using KDScorpionEngine.Exceptions;
    using Moq;
    using Raptor.Content;
    using Raptor.Graphics;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="KDScorpionEngine.Entities.Entity"/> class.
    /// </summary>
    public class EntityTests
    {
        private const string TextureName = "test-texture";
        private const string TextureAtlasName = "test-atlas-texture";
        private const string TextureAtlasPath = @"C:\temp\test-atlas-texture";
        private readonly Mock<ITexture> mockWholeTexture;
        private readonly Mock<ITexture> mockTextureAtlas;
        private readonly Mock<IContentLoader> mockContentLoader;
        private readonly Mock<ILoader<ITexture>> mockTextureLoader;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityTests"/> class.
        /// </summary>
        public EntityTests()
        {
            this.mockWholeTexture = new Mock<ITexture>();
            this.mockTextureAtlas = new Mock<ITexture>();

            this.mockContentLoader = new Mock<IContentLoader>();
            this.mockContentLoader.Setup(m => m.Load<ITexture>(TextureName)).Returns(this.mockWholeTexture.Object);
            this.mockContentLoader.Setup(m => m.Load<ITexture>(TextureAtlasName)).Returns(this.mockTextureAtlas.Object);

            this.mockTextureLoader = new Mock<ILoader<ITexture>>();
            this.mockTextureLoader.Setup(m => m.Load(It.IsAny<string>())).Returns(this.mockWholeTexture.Object);
        }

        #region Ctor Tests
        [Fact]
        public void Ctor_WhenCreatomg_RenderSectionIsNotNull()
        {
            // Arrang & Act
            var entity = CreateEntity();

            // Assert
            Assert.NotNull(entity.RenderSection);
        }

        [Fact]
        public void Ctor_WhenOnlyUsingName_ProperlySetsUpObjectState()
        {
            // Arrang & Act
            var entity = new Entity("test-entity");

            // Assert
            Assert.Equal("test-entity", entity.RenderSection.TextureName);
            Assert.Equal(TextureType.WholeTexture, entity.RenderSection.TypeOfTexture);
        }
        #endregion

        #region Prop Tests
        [Fact]
        public void Behaviors_WhenCreatingEntity_PropertyInstantiated()
        {
            // Arrange
            var entity = CreateEntity();

            // Act & Assert
            Assert.NotNull(entity.Behaviors);
        }

        [Fact]
        public void Visible_WhenGettingAndSettingValue_ValueProperlySet()
        {
            // Arrange
            var entity = CreateEntity();
            entity.Visible = false;

            var expected = false;

            // Act
            var actual = entity.Visible;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Visible_WhenSettingValue_InvokesOnHideEvent()
        {
            // Arrange
            var entity = CreateEntity();
            var eventRaised = false;
            entity.OnHide += (sender, e) => { eventRaised = true; };

            // Act
            entity.Visible = false;

            // Assert
            Assert.True(eventRaised);
        }

        [Fact]
        public void Visible_WhenSettingValue_InvokesOnShowEvent()
        {
            // Arrange
            var entity = CreateEntity();
            entity.Visible = false;

            var eventRaised = false;
            entity.OnShow += (sender, e) => { eventRaised = true; };

            // Act
            entity.Visible = true;

            // Assert
            Assert.True(eventRaised);
        }

        [Fact]
        public void Visible_WhenGoingFromInvisibleToVisible_InvokesOnShowEvent()
        {
            // Arrange
            var entity = CreateEntity();
            entity.Visible = false;

            var eventRaised = false;
            entity.OnShow += (sender, e) => { eventRaised = true; };

            // Act
            entity.Visible = true;

            // Assert
            Assert.True(eventRaised);
        }

        [Fact]
        public void Visible_WhenGoingFromInvisibleToVisible_DoesNotInvokesOnShowEvent()
        {
            // Arrange
            var entity = CreateEntity();
            entity.Visible = true;

            var eventRaised = false;
            entity.OnShow += (sender, e) => { eventRaised = true; };

            // Act
            entity.Visible = false;

            // Assert
            Assert.False(eventRaised);
        }

        [Fact]
        public void DebugDrawEnabled_WhenGettingDefaultValue_ReturnsTrue()
        {
            // Arrange
            var entity = CreateEntity();

            // Act & Assert
            Assert.False(entity.DebugDrawEnabled);
        }

        [Fact]
        public void DebugDrawEnabled_WhenSettingToFalse_ReturnsFalse()
        {
            // Arrange
            var entity = CreateEntity();

            entity.DebugDrawEnabled = false;

            // Act & Assert
            Assert.False(entity.DebugDrawEnabled);
        }

        [Fact]
        public void DebugDrawColor_WhenSettingValue_ReturnsCorrectValue()
        {
            // Arrange & Act
            var entity = CreateEntity();
            entity.DebugDrawColor = Color.FromArgb(11, 22, 33, 44);

            // Assert
            Assert.Equal(Color.FromArgb(11, 22, 33, 44), entity.DebugDrawColor);
        }

        [Fact]
        public void Behaviors_WhenSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var mockBehavior = new Mock<IBehavior>();

            var entity = CreateEntity();
            var expected = new EntityBehaviors
            {
                mockBehavior.Object,
            };

            // Act
            entity.Behaviors = new EntityBehaviors() { mockBehavior.Object };
            var actual = entity.Behaviors;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Position_WhenSettingValueAfterInitalized_ReturnsCorrectValue()
        {
            // Arrange
            var entity = CreateEntity();

            var expected = new Vector2(123, 456);

            // Act
            entity.Position = new Vector2(123, 456);
            var actual = entity.Position;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Position_WhenSettingValueBeforeInitialized_ReturnsCorrectValue()
        {
            // Arrange
            var entity = CreateEntity();
            var expected = new Vector2(123, 456);

            // Act
            entity.Position = new Vector2(123, 456);
            var actual = entity.Position;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Angle_WhenSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var entity = CreateEntity();

            // Act
            entity.Angle = 45f;

            // Assert
            Assert.Equal(45f, entity.Angle);
        }

        [Fact]
        public void Texture_WhenUsingWholeTexture_ReturnsCorrectTexture()
        {
            // Arange
            var entity = CreateEntity();
            entity.RenderSection.TextureName = TextureName;

            // Act
            entity.LoadContent(this.mockContentLoader.Object);

            // Assert
            Assert.Same(entity.Texture, this.mockWholeTexture.Object);
        }

        [Fact]
        public void Texture_WhenUsingSubTexture_ReturnsCorrectTexture()
        {
            // Arange
            var entity = CreateEntity();
            entity.RenderSection.TextureName = TextureAtlasName;
            entity.RenderSection.TypeOfTexture = TextureType.SubTexture;

            var subTextureData = new[]
            {
                new AtlasSubTextureData()
                {
                    Name = "sub-texture",
                    Bounds = new Rectangle(10, 20, 30, 40),
                    FrameIndex = -1,
                }
            };

            entity.AtlasData = new AtlasData(subTextureData, this.mockTextureAtlas.Object, TextureAtlasName, TextureAtlasPath);

            // Act
            entity.LoadContent(this.mockContentLoader.Object);

            // Assert
            Assert.Same(entity.Texture, this.mockTextureAtlas.Object);
        }

        [Fact]
        public void Texture_WhenSettingValueAsWholeTextureType_ReturnsCorrectTexture()
        {
            // Arrange
            var entity = CreateEntity();
            entity.RenderSection.TypeOfTexture = TextureType.WholeTexture;

            var subTextureData = new[]
            {
                new AtlasSubTextureData()
                {
                    Name = "sub-texture",
                    Bounds = new Rectangle(10, 20, 30, 40),
                    FrameIndex = -1,
                }
            };

            entity.AtlasData = new AtlasData(subTextureData, this.mockTextureAtlas.Object, TextureAtlasName, TextureAtlasPath);

            // Act
            entity.Texture = this.mockWholeTexture.Object;

            // Assert
            Assert.Same(entity.Texture, this.mockWholeTexture.Object);
            Assert.NotSame(entity.AtlasData.Texture, this.mockWholeTexture.Object);
        }

        [Fact]
        public void Texture_WhenTextureTypeIsUnknown_ThrowsExcetion()
        {
            // Arrange
            var entity = CreateEntity();
            entity.RenderSection.TypeOfTexture = (TextureType)44;

            // Act & Assert
            AssertHelpers.ThrowsWithMessage<TextureTypeException>(() =>
            {
                _ = entity.Texture;
            }, $"Unknown '{nameof(TextureType)}' value of '44'.");
        }

        [Fact]
        public void Texture_WhenAtlasDataIsNotSetup_ThrowsException()
        {
            // Arrange
            var entity = CreateEntity();
            entity.RenderSection.TypeOfTexture = TextureType.SubTexture;

            // Act & Assert
            AssertHelpers.ThrowsWithMessage<InvalidOperationException>(() =>
            {
                entity.Texture = new Mock<ITexture>().Object;
            }, "The atlas data must not be null when setting the texture for sub texture type.");
        }

        [Fact]
        public void Texture_WhenAtlasTextureIsNull_ThrowsException()
        {
            // Arrange
            var entity = CreateEntity();
            entity.RenderSection.TypeOfTexture = TextureType.SubTexture;
            var subTextureData = new[]
{
                new AtlasSubTextureData()
                {
                    Name = "sub-texture",
                    Bounds = new Rectangle(10, 20, 30, 40),
                    FrameIndex = -1,
                }
            };

            entity.AtlasData = new AtlasData(subTextureData, this.mockTextureAtlas.Object, TextureAtlasName, TextureAtlasPath);

            // Act & Assert
            AssertHelpers.ThrowsWithMessage<InvalidOperationException>(() =>
            {
                entity.Texture = null;
            }, "The texture atlas must not be null.");
        }

        [Fact]
        public void Texture_WhenSettingValueAsSubTextureType_ReturnsCorrectValue()
        {
            // Arrange
            var entity = CreateEntity();
            entity.RenderSection.TypeOfTexture = TextureType.SubTexture;

            var otherAtlasTexture = new Mock<ITexture>();

            var subTextureData = new[]
            {
                new AtlasSubTextureData()
                {
                    Name = "sub-texture",
                    Bounds = new Rectangle(10, 20, 30, 40),
                    FrameIndex = -1,
                }
            };

            entity.AtlasData = new AtlasData(subTextureData, this.mockTextureAtlas.Object, TextureAtlasName, TextureAtlasPath);

            // Act
            entity.Texture = otherAtlasTexture.Object;

            // Assert
            Assert.Same(entity.AtlasData.Texture, otherAtlasTexture.Object);
        }

        [Fact]
        public void Texture_WithInvalidTextureType_ThrowsException()
        {
            // Arrange
            var entity = CreateEntity();
            entity.RenderSection.TypeOfTexture = (TextureType)33;

            // Act & Assert
            AssertHelpers.ThrowsWithMessage<TextureTypeException>(() =>
            {
                entity.Texture = new Mock<ITexture>().Object;
            }, $"Unknown '{nameof(TextureType)}' value of '33'.");
        }
        #endregion

        #region Method Tests
        [Fact]
        public void OnUpdate_WhenInvoked_UpdatesBehaviors()
        {
            // Arrange
            var mockBehavior = new Mock<IBehavior>();
            var entity = CreateEntity();
            entity.Behaviors.Add(mockBehavior.Object);
            var gameTime = new GameTime();
            gameTime.UpdateTotalGameTime(16);

            // Act
            entity.Update(gameTime);

            // Assert
            mockBehavior.Verify(m => m.Update(It.IsAny<GameTime>()), Times.Once());
        }

        [Fact]
        public void LoadContent_WithNullContentLoader_ThrowsException()
        {
            // Arrange
            var entity = CreateEntity();

            // Act & Assert
            AssertHelpers.ThrowsWithMessage<ArgumentNullException>(() =>
            {
                entity.LoadContent(null);
            }, "The parameter must not be null. (Parameter 'contentLoader')");
        }

        [Fact]
        public void LoadContent_WhenInvoked_SetsContentLoadedPropToTrue()
        {
            // Arange
            var entity = CreateEntity();
            entity.RenderSection.TextureName = TextureName;

            // Act
            entity.LoadContent(this.mockContentLoader.Object);

            // Assert
            Assert.True(entity.ContentLoaded);
        }
        #endregion

        /// <summary>
        /// Creates a new instance of <see cref="Entity"/> for the purpose of testing.
        /// </summary>
        /// <returns>An entity instance to test.</returns>
        private Entity CreateEntity() => new Entity(string.Empty);

        private static Texture CreateTexture()
        {
            var mockTexture = new Mock<ITexture>();
            mockTexture.SetupGet(m => m.Width).Returns(100);
            mockTexture.SetupGet(m => m.Height).Returns(50);

            // TODO: The arguments going into this will not work
            return new Texture("", "", Array.Empty<byte>(), 0, 0);
        }
    }
}
