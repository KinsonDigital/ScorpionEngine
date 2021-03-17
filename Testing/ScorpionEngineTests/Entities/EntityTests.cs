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
    using KDScorpionEngine.Graphics;
    using Moq;
    using Raptor.Content;
    using Raptor.Graphics;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="KDScorpionEngine.Entities.Entity"/> class.
    /// </summary>
    public class EntityTests
    {
        private const string WholeTextureName = "whole-texture";
        private const string TextureAtlasName = "test-atlas-texture";
        private const string SubTextureName = "sub-texture";
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
            this.mockContentLoader.Setup(m => m.Load<ITexture>(WholeTextureName)).Returns(this.mockWholeTexture.Object);
            this.mockContentLoader.Setup(m => m.Load<ITexture>(TextureAtlasName)).Returns(this.mockTextureAtlas.Object);

            this.mockContentLoader.Setup(m => m.Load<IAtlasData>(TextureAtlasName))
                .Returns(() =>
                {
                    var subTextureData = new AtlasSubTextureData[]
                    {
                        new AtlasSubTextureData()
                        {
                            Name = SubTextureName,
                            FrameIndex = -1,
                            Bounds = new Rectangle(11, 22, 33, 44),
                        },
                    };

                    var atlasData = new AtlasData(subTextureData, this.mockTextureAtlas.Object, TextureAtlasName, TextureAtlasPath);

                    return atlasData;
                });

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
            Assert.NotNull(entity.SectionToRender);
        }

        [Fact]
        public void Ctor_WhenOnlyUsingName_ProperlySetsUpObjectState()
        {
            // Arrang & Act
            var entity = new Entity("test-entity");

            // Assert
            Assert.Equal("test-entity", entity.SectionToRender.TextureName);
            Assert.Equal(TextureType.WholeTexture, entity.SectionToRender.TypeOfTexture);
        }

        [Fact]
        public void Ctor_WhenInvokingWithAtlasDataAndAnimator_CreatesEntity()
        {
            // Arrange
            var mockAtlasData = new Mock<IAtlasData>();
            var mockAnimator = new Mock<IAnimator>();

            // Act
            var entity = new Entity(TextureAtlasName, SubTextureName, mockAtlasData.Object, mockAnimator.Object);

            // Assert
            Assert.NotEqual(Guid.Empty, entity.ID);
            Assert.Same(mockAtlasData.Object, entity.AtlasData);
            Assert.Same(mockAnimator.Object, entity.SectionToRender.Animator);
            Assert.Equal(TextureAtlasName, entity.SectionToRender.TextureName);
            Assert.Equal(SubTextureName, entity.SectionToRender.SubTextureName);
            Assert.Equal(TextureType.SubTexture, entity.SectionToRender.TypeOfTexture);
        }

        [Fact]
        public void Ctor_WhenInvokingWithAtlasData_CreatesEntity()
        {
            // Arrange
            var mockAtlasData = new Mock<IAtlasData>();

            // Act
            var entity = new Entity(TextureAtlasName, SubTextureName, mockAtlasData.Object);

            // Assert
            Assert.NotEqual(Guid.Empty, entity.ID);
            Assert.Same(mockAtlasData.Object, entity.AtlasData);
            Assert.Equal(TextureAtlasName, entity.SectionToRender.TextureName);
            Assert.Equal(SubTextureName, entity.SectionToRender.SubTextureName);
            Assert.Equal(TextureType.SubTexture, entity.SectionToRender.TypeOfTexture);
        }

        [Fact]
        public void Ctor_WhenInvokingWithWholeTexture_CreatesEntity()
        {
            // Act
            var entity = new Entity(WholeTextureName, this.mockWholeTexture.Object);

            // Assert
            Assert.NotEqual(Guid.Empty, entity.ID);
            Assert.Same(this.mockWholeTexture.Object, entity.Texture);
            Assert.Equal(WholeTextureName, entity.SectionToRender.TextureName);
            Assert.Equal(string.Empty, entity.SectionToRender.SubTextureName);
            Assert.Equal(TextureType.WholeTexture, entity.SectionToRender.TypeOfTexture);
        }

        [Fact]
        public void Ctor_WhenInvokingWithOnlyAtlasTextureAndSubTextureNames_CreatesEntity()
        {
            // Arrange

            // Act
            var entity = new Entity(TextureAtlasName, SubTextureName);

            // Assert
            Assert.NotEqual(Guid.Empty, entity.ID);
            Assert.Equal(TextureAtlasName, entity.SectionToRender.TextureName);
            Assert.Equal(SubTextureName, entity.SectionToRender.SubTextureName);
            Assert.Equal(TextureType.SubTexture, entity.SectionToRender.TypeOfTexture);
        }

        [Fact]
        public void Ctor_WhenInvokingWithAnimator_CreatesEntity()
        {
            // Arrange
            var mockAnimator = new Mock<IAnimator>();

            // Act
            var entity = new Entity(TextureAtlasName, SubTextureName, mockAnimator.Object);

            // Assert
            Assert.NotEqual(Guid.Empty, entity.ID);
            Assert.Same(mockAnimator.Object, entity.SectionToRender.Animator);
            Assert.Equal(TextureAtlasName, entity.SectionToRender.TextureName);
            Assert.Equal(SubTextureName, entity.SectionToRender.SubTextureName);
            Assert.Equal(TextureType.SubTexture, entity.SectionToRender.TypeOfTexture);
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
        public void Visible_WhenGoingFromVisibleToHidden_InvokesOnHideEvent()
        {
            // Arrange
            var entity = CreateEntity();

            // Act & Assert
            Assert.Raises<EventArgs>(
                e => entity.OnHide += e,
                e => entity.OnHide -= e,
                () => entity.Visible = false);
        }

        [Fact]
        public void Visible_WhenGoingFromHiddenToVisible_InvokesOnShowEvent()
        {
            // Arrange
            var entity = CreateEntity();
            entity.Visible = false;

            // Act & Assert
            Assert.Raises<EventArgs>(
                e => entity.OnShow += e,
                e => entity.OnShow -= e,
                () =>
                {
                    entity.Visible = true;
                });
        }

        [Fact]
        public void Visible_WhenGoingFromHiddenToVisibleWithNoRegisteredEvent_DoesNotThrowException()
        {
            // Arrange
            var entity = CreateEntity();
            entity.Visible = false;

            // Act & Assert
            AssertHelpers.DoesNotThrow<NullReferenceException>(() =>
            {
                entity.Visible = true;
            });
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
            entity.SectionToRender.TextureName = WholeTextureName;

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
            entity.SectionToRender.TextureName = TextureAtlasName;
            entity.SectionToRender.TypeOfTexture = TextureType.SubTexture;

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
            entity.SectionToRender.TypeOfTexture = TextureType.WholeTexture;

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
            entity.SectionToRender.TypeOfTexture = (TextureType)44;

            // Act & Assert
            AssertHelpers.ThrowsWithMessage<InvalidTextureTypeException>(() =>
            {
                _ = entity.Texture;
            }, $"Unknown '{nameof(TextureType)}' value of '44'.");
        }

        [Fact]
        public void Texture_WhenAtlasDataIsNotSetup_ThrowsException()
        {
            // Arrange
            var entity = CreateEntity();
            entity.SectionToRender.TypeOfTexture = TextureType.SubTexture;

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
            entity.SectionToRender.TypeOfTexture = TextureType.SubTexture;
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
            entity.SectionToRender.TypeOfTexture = TextureType.SubTexture;

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
        public void Texture_WhenGettingValueAsSubTextureTypeWithNullAtlasData_DoesNotThrowException()
        {
            // Arrange
            var entity = CreateEntity();
            entity.SectionToRender.TypeOfTexture = TextureType.SubTexture;

            var otherAtlasTexture = new Mock<ITexture>();

            ITexture actual = null;

            // Act & Assert
            AssertHelpers.DoesNotThrow<NullReferenceException>(() =>
            {
                actual = entity.Texture;
            });

            Assert.Null(actual);
        }

        [Fact]
        public void Texture_WithInvalidTextureType_ThrowsException()
        {
            // Arrange
            var entity = CreateEntity();
            entity.SectionToRender.TypeOfTexture = (TextureType)33;

            // Act & Assert
            AssertHelpers.ThrowsWithMessage<InvalidTextureTypeException>(() =>
            {
                entity.Texture = new Mock<ITexture>().Object;
            }, $"Unknown '{nameof(TextureType)}' value of '33'.");
        }

        [Fact]
        public void ID_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var entity = new Entity();

            // Act
            var actual = entity.ID;

            // Assert
            Assert.NotEqual(Guid.Empty, actual);
        }

        [Fact]
        public void Name_WhenEntityIsNonAnimated_ReturnsCorrectValue()
        {
            // Arrange
            var entity = CreateEntity();
            entity.SectionToRender.Animator = null;
            entity.SectionToRender.TextureName = "texture";

            // Act
            var actual = entity.Name;

            // Assert
            Assert.Equal("texture", actual);
        }

        [Theory]
        [InlineData("sub-texture", "sub-texture")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public void Name_WhenEntityIsAnimated_ReturnsCorrectValue(string subTextureName, string expected)
        {
            // Arrange
            var entity = CreateEntity();
            entity.SectionToRender.Animator = new Animator();
            entity.SectionToRender.SubTextureName = subTextureName;

            // Act
            var actual = entity.Name;

            // Assert
            Assert.Equal(expected, actual);
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
            gameTime.AddTime(16);

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
        public void LoadContent_WhenContentIsLoaded_SkipsLoadingOfContent()
        {
            // Arrange
            var entity = CreateEntity();
            entity.SectionToRender.TextureName = WholeTextureName;
            entity.SectionToRender.TypeOfTexture = TextureType.WholeTexture;

            // Act
            entity.LoadContent(this.mockContentLoader.Object);

            // Set the texture to null to see if the content loader is used a second time
            entity.Texture = null;
            entity.LoadContent(this.mockContentLoader.Object);

            // Assert
            this.mockContentLoader.Verify(m => m.Load<ITexture>(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void LoadContent_WhenInvoked_SetsContentLoadedPropToTrue()
        {
            // Arange
            var entity = CreateEntity();
            entity.SectionToRender.TextureName = WholeTextureName;

            // Act
            entity.LoadContent(this.mockContentLoader.Object);

            // Assert
            Assert.True(entity.ContentLoaded);
        }

        [Fact]
        public void LoadContent_WhenInvoked_LoadsAtlasData()
        {
            // Arrange
            var entity = CreateEntity();
            entity.SectionToRender.TextureName = TextureAtlasName;
            entity.SectionToRender.TypeOfTexture = TextureType.SubTexture;

            // Act
            entity.LoadContent(this.mockContentLoader.Object);

            // Assert
            this.mockContentLoader.Verify(m => m.Load<IAtlasData>(TextureAtlasName), Times.Once());
        }

        [Fact]
        public void LoadContent_WithSetAnimator_SetsAnimatorFrames()
        {
            // Arrange
            var mockAnimator = new Mock<IAnimator>();
            mockAnimator.SetupProperty(p => p.Frames);

            var entity = CreateEntity();
            entity.SectionToRender.TextureName = TextureAtlasName;
            entity.SectionToRender.SubTextureName = SubTextureName;
            entity.SectionToRender.TypeOfTexture = TextureType.SubTexture;
            entity.SectionToRender.Animator = mockAnimator.Object;

            // Act
            entity.LoadContent(this.mockContentLoader.Object);

            // Assert
            mockAnimator.VerifySet(p =>
            {
                p.Frames = new[] { new Rectangle(11, 22, 33, 44) }.ToReadOnlyCollection();
            },
            Times.Once());
        }

        [Fact]
        public void LoadContent_WithInvalidTextureType_ThrowsException()
        {
            // Arrange
            var mockAnimator = new Mock<IAnimator>();
            mockAnimator.SetupProperty(p => p.Frames);

            var entity = CreateEntity();
            entity.SectionToRender.TypeOfTexture = (TextureType)44;

            // Act & Assert
            AssertHelpers.ThrowsWithMessage<InvalidTextureTypeException>(() =>
            {
                entity.LoadContent(this.mockContentLoader.Object);
            }, "The texture type is invalid.");
        }

        [Fact]
        public void Init_WhenInvoked_SetsAsContentNotLoaded()
        {
            // Arrange
            var entity = CreateEntity();
            entity.SectionToRender.TextureName = WholeTextureName;
            entity.LoadContent(this.mockContentLoader.Object);

            // Act
            entity.Init();

            // Assert
            Assert.False(entity.ContentLoaded);
        }

        [Fact]
        public void Update_WhenDisabled_DoesNotUpdateBehaviorsAndAnimator()
        {
            // Arrange
            var mockBehavior = new Mock<IBehavior>();
            var mockAnimator = new Mock<IAnimator>();

            var entity = CreateEntity();
            entity.Enabled = false;
            entity.Behaviors.Add(mockBehavior.Object);
            entity.SectionToRender.Animator = mockAnimator.Object;

            // Act
            entity.Update(new GameTime());

            // Assert
            mockBehavior.Verify(m => m.Update(It.IsAny<GameTime>()), Times.Never());
            mockAnimator.Verify(m => m.Update(It.IsAny<GameTime>()), Times.Never());
        }

        [Fact]
        public void Update_WhenEnabled_UpdatesBehaviors()
        {
            // Arrange
            var mockBehavior = new Mock<IBehavior>();

            var entity = CreateEntity();
            entity.Behaviors.Add(mockBehavior.Object);

            // Act
            entity.Update(new GameTime());

            // Assert
            mockBehavior.Verify(m => m.Update(It.IsAny<GameTime>()), Times.Once());
        }

        [Fact]
        public void Update_WhenEnabled_UpdatesAnimator()
        {
            // Arrange
            var mockAnimator = new Mock<IAnimator>();

            var entity = CreateEntity();
            entity.SectionToRender.Animator = mockAnimator.Object;

            // Act
            entity.Update(new GameTime());

            // Assert
            mockAnimator.Verify(m => m.Update(It.IsAny<GameTime>()), Times.Once());
        }

        [Fact]
        public void Update_WithAnimatingEntity_UpdatesRenderBounds()
        {
            // Arrange
            var expected = new Rectangle(11, 22, 33, 44);
            var mockAnimator = new Mock<IAnimator>();
            mockAnimator.Setup(p => p.CurrentFrameBounds).Returns(new Rectangle(11, 22, 33, 44));

            var entity = CreateEntity();
            entity.SectionToRender.TypeOfTexture = TextureType.SubTexture;
            entity.SectionToRender.Animator = mockAnimator.Object;

            // Act
            entity.Update(new GameTime());

            // Assert
            Assert.Equal(expected, entity.SectionToRender.RenderBounds);
        }

        [Fact]
        public void UnloadContent_WhenInvoked_UnloadsContent()
        {
            // Arrange
            var entity = CreateEntity();
            entity.AtlasData = new Mock<IAtlasData>().Object;
            entity.Texture = new Mock<ITexture>().Object;

            // Act
            entity.UnloadContent(this.mockContentLoader.Object);

            // Assert
            Assert.Null(entity.AtlasData);
            Assert.Null(entity.Texture);
        }

        [Fact]
        public void AddChildEntity_WhenInvoked_AddsEntityToParentEntity()
        {
            // Arrange
            var childEntity = new Mock<IEntity>();
            var entity = CreateEntity();

            // Act
            entity.AddChildEntity(childEntity.Object);

            // Assert
            Assert.Single(entity.Entities);
            Assert.Same(childEntity.Object, entity.Entities[0]);
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
