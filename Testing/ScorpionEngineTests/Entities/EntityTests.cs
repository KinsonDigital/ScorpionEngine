// <copyright file="EntityTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Entities
{
    using System;
    using System.Collections.ObjectModel;
    using System.Numerics;
    using KDScorpionEngine.Behaviors;
    using KDScorpionEngine.Exceptions;
    using KDScorpionEngineTests.Fakes;
    using Moq;
    using Raptor;
    using Raptor.Content;
    using Raptor.Graphics;
    using Raptor.Plugins;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="KDScorpionEngine.Entities.Entity"/> class.
    /// </summary>
    public class EntityTests : IDisposable
    {
        #region Private Fields
        private Mock<IPhysicsBody> _mockPhysicsBody;
        private Mock<IDebugDraw> _mockDebugDraw;
        private Mock<IContentLoader> _contentLoader;
        #endregion

        #region Constructors
        public EntityTests()
        {
            _mockPhysicsBody = new Mock<IPhysicsBody>();
            _mockPhysicsBody.SetupProperty(m => m.X);
            _mockPhysicsBody.SetupProperty(m => m.Y);
            _mockPhysicsBody.Setup(m => m.XVertices).Returns(new ReadOnlyCollection<float>( new [] { -50f, 50f, 50f, -50f }));
            _mockPhysicsBody.Setup(m => m.YVertices).Returns(new ReadOnlyCollection<float>(new float[] { -50, -50, 50, 50 }));

            _mockDebugDraw = new Mock<IDebugDraw>();
            _mockDebugDraw.Setup(m => m.Draw(It.IsAny<IRenderer>(), It.IsAny<IPhysicsBody>()));

            _contentLoader = new Mock<IContentLoader>();
        }
        #endregion

        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvokingWithStaticBodyValue_ProperlySetsBodyAsStatic()
        {
            // Arrange
            var fakeEntity = new FakeEntity(true);
            var expected = true;

            // Act
            var actual = fakeEntity.IsStatic;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Ctor_WhenInvokingWithPositionParam_ProperlySetsPositionProperty()
        {
            // Arrange
            var fakeEntity = new FakeEntity(new Vector2(11, 22));
            var expected = new Vector2(11, 22);

            // Act
            var actual = fakeEntity.Position;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Ctor_WhenInvokingWithVertices_ProperlySetsUpObject()
        {
            // Arrange
            var halfWidth = 50;
            var halfHeight = 25;
            var position = new Vector2(10, 20);
            var vertices = new Vector2[4]
            {
                new Vector2(position.X - halfWidth, position.Y - halfHeight),
                new Vector2(position.X + halfWidth, position.Y - halfHeight),
                new Vector2(position.X + halfWidth, position.Y + halfHeight),
                new Vector2(position.X - halfWidth, position.Y + halfHeight),
            };
            var expectedPosition = new Vector2(10, 20);

            // Act
            var fakeEntity = new FakeEntity(vertices, position);
            var actualPosition = fakeEntity.Position;

            // Assert
            Assert.Equal(expectedPosition, actualPosition);
        }
        #endregion

        #region Prop Tests
        [Fact]
        public void Behaviors_WhenCreatingEntity_PropertyInstantiated()
        {
            // Arrange
            var fakeEntity = new FakeEntity(_mockPhysicsBody.Object);

            // Act
            var actual = fakeEntity.Behaviors;

            // Assert
            Assert.NotNull(actual);
        }

        [Fact]
        public void Visible_WhenGettingAndSettingValue_ValueProperlySet()
        {
            // Arrange
            var fakeEntity = new FakeEntity(_mockPhysicsBody.Object)
            {
                Visible = false
            };
            var expected = false;

            // Act
            var actual = fakeEntity.Visible;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Visible_WhenSettingValue_InvokesOnHideEvent()
        {
            // Arrange
            var fakeEntity = new FakeEntity(_mockPhysicsBody.Object);
            var eventRaised = false;
            fakeEntity.OnHide += (sender, e) => { eventRaised = true; };

            // Act
            fakeEntity.Visible = false;

            // Assert
            Assert.True(eventRaised);
        }

        [Fact]
        public void Visible_WhenSettingValue_InvokesOnShowEvent()
        {
            // Arrange
            var fakeEntity = new FakeEntity(_mockPhysicsBody.Object)
            {
                Visible = false
            };
            var eventRaised = false;
            fakeEntity.OnShow += (sender, e) => { eventRaised = true; };

            // Act
            fakeEntity.Visible = true;

            // Assert
            Assert.True(eventRaised);
        }

        [Fact]
        public void Visible_WhenGoingFromInvisibleToVisible_InvokesOnShowEvent()
        {
            // Arrange
            var fakeEntity = new FakeEntity(_mockPhysicsBody.Object)
            {
                Visible = false
            };
            var eventRaised = false;
            fakeEntity.OnShow += (sender, e) => { eventRaised = true; };

            // Act
            fakeEntity.Visible = true;

            // Assert
            Assert.True(eventRaised);
        }

        [Fact]
        public void Visible_WhenGoingFromInvisibleToVisible_DoesNotInvokesOnShowEvent()
        {
            // Arrange
            var fakeEntity = new FakeEntity(_mockPhysicsBody.Object)
            {
                Visible = true
            };
            var eventRaised = false;
            fakeEntity.OnShow += (sender, e) => { eventRaised = true; };

            // Act
            fakeEntity.Visible = false;

            // Assert
            Assert.False(eventRaised);
        }

        [Fact]
        public void Bounds_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var fakeEntity = new FakeEntity(_mockPhysicsBody.Object);
            fakeEntity.Initialize();

            var expected = new Rect(0, 0, 100, 100);

            // Act
            var actual = fakeEntity.Bounds;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BoundsWidth_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var fakeEntity = new FakeEntity(_mockPhysicsBody.Object);

            fakeEntity.Initialize();
            var expected = 100;

            // Act
            var actual = fakeEntity.BoundsWidth;

            // Assert
            Assert.Equal(expected, actual);
        }

        // [Fact]
        public void BoundsWidth_WhenGettingValueWithNullBody_ReturnsZero()
        {
            // Arrange
            float[] nums = null;
            _mockPhysicsBody.Setup(m => m.XVertices).Returns(new ReadOnlyCollection<float>(nums));

            var fakeEntity = new FakeEntity(_mockPhysicsBody.Object);

            fakeEntity.Initialize();
            var expected = 0;

            // Act
            var actual = fakeEntity.BoundsWidth;

            // Assert
            Assert.Equal(expected, actual);
        }

        // [Fact]
        public void BoundsHeight_WhenGettingValueWithNullBody_ReturnsZero()
        {
            // Arrange
            float[] nums = null;
            _mockPhysicsBody.Setup(m => m.YVertices).Returns(new ReadOnlyCollection<float>(nums));

            var fakeEntity = new FakeEntity(_mockPhysicsBody.Object);

            fakeEntity.Initialize();
            var expected = 0;

            // Act
            var actual = fakeEntity.BoundsHeight;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BoundsHeight_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var fakeEntity = new FakeEntity(_mockPhysicsBody.Object);

            fakeEntity.Initialize();
            var expected = 100;

            // Act
            var actual = fakeEntity.BoundsHeight;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BoundsHalfWidth_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var fakeEntity = new FakeEntity(_mockPhysicsBody.Object);

            fakeEntity.Initialize();
            var expected = 50;

            // Act
            var actual = fakeEntity.BoundsHalfWidth;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BoundsHalfHeight_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var fakeEntity = new FakeEntity(_mockPhysicsBody.Object);

            fakeEntity.Initialize();
            var expected = 50f;

            // Act
            var actual = fakeEntity.BoundsHalfHeight;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Texture_WhenSettingAndeGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var vertices = new[] { Vector2.Zero };
            var fakeEntity = new FakeEntity(vertices, It.IsAny<Vector2>())
            {
                Texture = CreateTexture()
            };

            // Act
            var actual = fakeEntity.Texture;

            // Assert
            Assert.NotNull(actual);
        }

        [Fact]
        public void DebugDrawEnabled_WhenGettingDefaultValue_ReturnsTrue()
        {
            // Arrange
            var fakeEntity = new FakeEntity(null);

            // Act & Assert
            Assert.True(fakeEntity.DebugDrawEnabled);
        }

        [Fact]
        public void DebugDrawEnabled_WhenSettingToFalse_ReturnsFalse()
        {
            // Arrange
            var fakeEntity = new FakeEntity(null)
            {
                DebugDrawEnabled = false
            };

            // Act & Assert
            Assert.False(fakeEntity.DebugDrawEnabled);
        }

        [Fact]
        public void DebugDrawColor_WhenSettingValue_ReturnsCorrectValue()
        {
            // Arrange & Act
            var fakeEntity = new FakeEntity(null)
            {
                DebugDrawColor = new GameColor(11, 22, 33, 44)
            };

            // Assert
            Assert.Equal(new GameColor(11, 22, 33, 44), fakeEntity.DebugDrawColor);
        }

        [Fact]
        public void Behaviors_WhenSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var mockBehavior = new Mock<IBehavior>();

            var fakeEntity = new FakeEntity(_mockPhysicsBody.Object);
            var expected = new EntityBehaviors
            {
                mockBehavior.Object
            };

            // Act
            fakeEntity.Behaviors = new EntityBehaviors() { mockBehavior.Object };
            var actual = fakeEntity.Behaviors;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Position_WhenSettingValueAfterInitalized_ReturnsCorrectValue()
        {
            // Arrange
            var fakeEntity = new FakeEntity(_mockPhysicsBody.Object);
            fakeEntity.Initialize();

            var expected = new Vector2(123, 456);

            // Act
            fakeEntity.Position = new Vector2(123, 456);
            var actual = fakeEntity.Position;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Position_WhenSettingValueBeforeInitialized_ReturnsCorrectValue()
        {
            // Arrange
            var fakeEntity = new FakeEntity(_mockPhysicsBody.Object);
            var expected = new Vector2(123, 456);

            // Act
            fakeEntity.Position = new Vector2(123, 456);
            var actual = fakeEntity.Position;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Vertices_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var entity = new FakeEntity(true);
            var expected = new Vector2[]
            {
                new Vector2(11, 22),
                new Vector2(33, 44),
                new Vector2(55, 66)
            };

            // Act
            entity.Vertices = expected;
            var actual = entity.Vertices;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Vertices_WhenSettingValueAfterInit_ThrowsException()
        {
            // Arrange
            var entity = new FakeEntity(_mockPhysicsBody.Object);
            entity.Initialize();

            var expected = new Vector2[]
            {
                new Vector2(11, 22),
                new Vector2(33, 44),
                new Vector2(55, 66)
            };

            // Act/Assert
            Assert.Throws<EntityAlreadyInitializedException>(() => entity.Vertices = expected);
        }

        [Fact]
        public void Vertices_WhenGettingValueBeforeInit_ReturnsCorrectValue()
        {
            // Arrange
            var vertices = new Vector2[]
            {
                new Vector2(11, 22),
                new Vector2(33, 44),
                new Vector2(55, 66)
            };

            var entity = new FakeEntity(vertices, It.IsAny<Vector2>());
            var expected = new Vector2[]
            {
                new Vector2(11, 22),
                new Vector2(33, 44),
                new Vector2(55, 66)
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
            var entity = new FakeEntity(_mockPhysicsBody.Object);

            var expected = new Vector2[]
            {
                new Vector2(-50, -50),
                new Vector2(50, -50),
                new Vector2(50, 50),
                new Vector2(-50, 50)
            };
            entity.Initialize();

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
            var fakeEntity = new FakeEntity(_mockPhysicsBody.Object);
            fakeEntity.Behaviors.Add(mockBehavior.Object);
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };

            // Act
            fakeEntity.Update(engineTime);

            // Assert
            mockBehavior.Verify(m => m.Update(It.IsAny<EngineTime>()), Times.Once());
        }

        [Fact]
        public void LoadContent_WhenInvoked_SetsContentLoadedPropToTrue()
        {
            // Arange
            var entity = new FakeEntity(true);
            var expected = true;

            // Act
            entity.LoadContent(new ContentLoader(_contentLoader.Object));
            var actual = entity.ContentLoaded;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion

        #region Public Methods
        public void Dispose()
        {
            _mockPhysicsBody = null;
            _mockDebugDraw = null;
            _contentLoader = null;
        }
        #endregion

        #region Private Methods
        private static Texture CreateTexture()
        {
            var mockTexture = new Mock<ITexture>();
            mockTexture.SetupGet(m => m.Width).Returns(100);
            mockTexture.SetupGet(m => m.Height).Returns(50);

            return new Texture(mockTexture.Object);
        }
        #endregion
    }
}
