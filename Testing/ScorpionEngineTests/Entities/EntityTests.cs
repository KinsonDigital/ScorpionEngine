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
    using KDScorpionEngineTests.Fakes;
    using Moq;
    using Raptor.Content;
    using Raptor.Graphics;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="KDScorpionEngine.Entities.Entity"/> class.
    /// </summary>
    public class EntityTests : IDisposable
    {
        private Mock<IContentLoader> mockContentLoader;
        private Mock<ITexture> mockTexture;

        public EntityTests()
        {
            this.mockContentLoader = new Mock<IContentLoader>();
            this.mockTexture = new Mock<ITexture>();
        }

        #region Prop Tests
        [Fact]
        public void Behaviors_WhenCreatingEntity_PropertyInstantiated()
        {
            // Arrange
            var entity = CreateEntity();;

            // Act
            var actual = entity.Behaviors;

            // Assert
            Assert.NotNull(actual);
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
            var entity = CreateEntity();;
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
        public void Bounds_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var entity = CreateEntity();;

            var expected = new Rectangle(0, 0, 100, 100);

            // Act
            var actual = entity.Bounds;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BoundsWidth_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var entity = CreateEntity();;

            var expected = 100;

            // Act
            var actual = entity.BoundsWidth;

            // Assert
            Assert.Equal(expected, actual);
        }

        // [Fact]
        public void BoundsWidth_WhenGettingValueWithNullBody_ReturnsZero()
        {
            // Arrange
            var entity = CreateEntity();;

            var expected = 0;

            // Act
            var actual = entity.BoundsWidth;

            // Assert
            Assert.Equal(expected, actual);
        }

        // [Fact]
        public void BoundsHeight_WhenGettingValueWithNullBody_ReturnsZero()
        {
            // Arrange
            var entity = CreateEntity();;

            var expected = 0;

            // Act
            var actual = entity.BoundsHeight;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BoundsHeight_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var entity = CreateEntity();;

            var expected = 100;

            // Act
            var actual = entity.BoundsHeight;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BoundsHalfWidth_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var entity = CreateEntity();;

            var expected = 50;

            // Act
            var actual = entity.BoundsHalfWidth;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BoundsHalfHeight_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var entity = CreateEntity();;

            var expected = 50f;

            // Act
            var actual = entity.BoundsHalfHeight;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DebugDrawEnabled_WhenGettingDefaultValue_ReturnsTrue()
        {
            // Arrange
            var entity = CreateEntity();;

            // Act & Assert
            Assert.True(entity.DebugDrawEnabled);
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

            var entity = CreateEntity();;
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
            var entity = CreateEntity();;

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
            var entity = CreateEntity();;
            var expected = new Vector2(123, 456);

            // Act
            entity.Position = new Vector2(123, 456);
            var actual = entity.Position;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Vertices_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var entity = CreateEntity();
            var expected = new Vector2[]
            {
                new Vector2(11, 22),
                new Vector2(33, 44),
                new Vector2(55, 66),
            };

            // Act
            entity.Vertices = expected;
            var actual = entity.Vertices;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Vertices_WhenGettingValueBeforeInit_ReturnsCorrectValue()
        {
            // Arrange
            var vertices = new Vector2[]
            {
                new Vector2(11, 22),
                new Vector2(33, 44),
                new Vector2(55, 66),
            };

            var entity = CreateEntity();
            var expected = new Vector2[]
            {
                new Vector2(11, 22),
                new Vector2(33, 44),
                new Vector2(55, 66),
            };

            // Act
            var actual = entity.Vertices;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Vertices_WhenGettingValueBeforeAfterInit_ReturnsCorrectValue()
        {
            // Arrange
            var entity = CreateEntity();

            var expected = new Vector2[]
            {
                new Vector2(-50, -50),
                new Vector2(50, -50),
                new Vector2(50, 50),
                new Vector2(-50, 50),
            };

            // Act
            var actual = entity.Vertices;

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
            gameTime.UpdateTotalGameTime(16);

            // Act
            entity.Update(gameTime);

            // Assert
            mockBehavior.Verify(m => m.Update(It.IsAny<GameTime>()), Times.Once());
        }

        [Fact]
        public void LoadContent_WhenInvoked_SetsContentLoadedPropToTrue()
        {
            // Arange
            var entity = CreateEntity();
            var expected = true;

            // Act
            entity.LoadContent(null);
            var actual = entity.ContentLoaded;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion

        /// <inheritdoc/>
        public void Dispose()
        {
            this.mockContentLoader = null;
        }

        private FakeEntity CreateEntity() => new FakeEntity(this.mockTexture.Object);

        private static Texture CreateTexture()
        {
            var mockTexture = new Mock<ITexture>();
            mockTexture.SetupGet(m => m.Width).Returns(100);
            mockTexture.SetupGet(m => m.Height).Returns(50);

            // TODO: The arguments going into this will not work
            return new Texture("", new byte[0], 0, 0);
        }
    }
}
