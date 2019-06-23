using System;
using Moq;
using Xunit;
using KDScorpionCore;
using KDScorpionCore.Content;
using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Behaviors;
using KDScorpionEngine.Exceptions;
using KDScorpionEngineTests.Fakes;
using PluginSystem;
using KDScorpionCore.Physics;

namespace KDScorpionEngineTests.Entities
{
    public class EntityTests : IDisposable
    {
        #region Fields
        private Mock<IPluginLibrary> _mockEnginePluginLib;
        private Mock<IPluginLibrary> _mockPhysicsPluginLib;
        private Mock<IPhysicsBody> _mockPhysicsBody;
        private Mock<IDebugDraw> _mockDebugDraw;
        private ContentLoader _contentLoader;
        private Plugins _plugins;
        #endregion


        #region Constructors
        public EntityTests()
        {
            _mockPhysicsBody = new Mock<IPhysicsBody>();
            _mockPhysicsBody.Setup(m => m.XVertices).Returns(new float[] { -50, 50, 50, -50 });
            _mockPhysicsBody.Setup(m => m.YVertices).Returns(new float[] { -50, -50, 50, 50 });

            _mockDebugDraw = new Mock<IDebugDraw>();
            _mockDebugDraw.Setup(m => m.Draw(It.IsAny<IRenderer>(), It.IsAny<IPhysicsBody>()));

            var mockContentLoader = new Mock<IContentLoader>();
            _contentLoader = new ContentLoader(mockContentLoader.Object);

            _mockEnginePluginLib = new Mock<IPluginLibrary>();
            _mockEnginePluginLib.Setup(m => m.LoadPlugin<IDebugDraw>()).Returns(() => _mockDebugDraw.Object);

            _mockPhysicsPluginLib = new Mock<IPluginLibrary>();
            _mockPhysicsPluginLib.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParams) => _mockPhysicsBody.Object);

            _plugins = new Plugins()
            {
                EnginePlugins = _mockEnginePluginLib.Object,
                PhysicsPlugins = _mockPhysicsPluginLib.Object
            };

            CorePluginSystem.SetPlugins(_plugins);
        }
        #endregion


        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvokingWithStaticBodyValue_ProperlySetsBodyAsStatic()
        {
            //Arrange
            var fakeEntity = new FakeEntity(true);
            var expected = true;

            //Act
            var actual = fakeEntity.IsStatic;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Ctor_WhenInvokingWithPositionParam_ProperlySetsPositionProperty()
        {
            //Arrange
            var fakeEntity = new FakeEntity(new Vector(11, 22));
            var expected = new Vector(11, 22);

            //Act
            var actual = fakeEntity.Position;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Ctor_WhenInvokingWithTexture_ProperlySetsUpObject()
        {
            //Arrange
            var mockTexture = new Mock<ITexture>();
            mockTexture.SetupGet(m => m.Width).Returns(100);
            mockTexture.SetupGet(m => m.Height).Returns(50);

            var texture = new Texture(mockTexture.Object);
            var fakeEntity = new FakeEntity(CreateTexture(), It.IsAny<Vector>());
            var expectedVertices = new Vector[]
            {
                new Vector(-50, -25),
                new Vector(50, -25),
                new Vector(50, 25),
                new Vector(-50, 25)
            };

            //Act
            var actualTexture = fakeEntity.Texture;

            //Assert
            Assert.NotNull(actualTexture);
        }


        [Fact]
        public void Ctor_WhenInvokingWithVertices_ProperlySetsUpObject()
        {
            //Arrange
            var halfWidth = 50;
            var halfHeight = 25;
            var position = new Vector(10, 20);
            var vertices = new Vector[4]
            {
                new Vector(position.X - halfWidth, position.Y - halfHeight),
                new Vector(position.X + halfWidth, position.Y - halfHeight),
                new Vector(position.X + halfWidth, position.Y + halfHeight),
                new Vector(position.X - halfWidth, position.Y + halfHeight),
            };
            var expectedPosition = new Vector(10, 20);

            //Act
            var fakeEntity = new FakeEntity(vertices, position);
            var actualPosition = fakeEntity.Position;

            //Assert
            Assert.Equal(expectedPosition, actualPosition);
        }


        [Fact]
        public void Ctor_WhenInvokingWithTextureAndVertices_ProperlySetsUpObject()
        {
            //Arrange
            _mockPhysicsBody.Setup(m => m.X).Returns(10);
            _mockPhysicsBody.Setup(m => m.Y).Returns(15);

            var mockTexture = new Mock<ITexture>();
            mockTexture.SetupGet(m => m.Width).Returns(100);
            mockTexture.SetupGet(m => m.Height).Returns(100);

            var texture = new Texture(mockTexture.Object);

            var halfWidth = 50;
            var halfHeight = 50;
            var position = new Vector(10, 15);
            var vertices = new Vector[4]
            {
                new Vector(position.X - halfWidth, position.Y - halfHeight),
                new Vector(position.X + halfWidth, position.Y - halfHeight),
                new Vector(position.X + halfWidth, position.Y + halfHeight),
                new Vector(position.X - halfWidth, position.Y + halfHeight),
            };
            var expectedPosition = new Vector(10, 15);

            //Act
            var fakeEntity = new FakeEntity(CreateTexture(), vertices, position)
            {
                Body = new PhysicsBody(It.IsAny<Vector[]>(), It.IsAny<Vector>())
            };

            var actualPosition = fakeEntity.Position;
            var actualTexture = fakeEntity.Texture;

            //Assert
            Assert.NotNull(actualTexture);
            Assert.Equal(expectedPosition, actualPosition);
        }
        #endregion


        #region Prop Tests
        [Fact]
        public void Behaviors_WhenCreatingEntity_PropertyInstantiated()
        {
            //Arrange
            var fakeEntity = new FakeEntity(CreateTexture(), It.IsAny<Vector>());

            //Act
            var actual = fakeEntity.Behaviors;

            //Assert
            Assert.NotNull(actual);
        }


        [Fact]
        public void Visible_WhenGettingAndSettingValue_ValueProperlySet()
        {
            //Arrange
            var fakeEntity = new FakeEntity(CreateTexture(), Vector.Zero)
            {
                Visible = false
            };
            var expected = false;

            //Act
            var actual = fakeEntity.Visible;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Visible_WhenSettingValue_InvokesOnHideEvent()
        {
            //Arrange
            var fakeEntity = new FakeEntity(CreateTexture(), It.IsAny<Vector>());
            var eventRaised = false;
            fakeEntity.OnHide += (sender, e) => { eventRaised = true; };

            //Act
            fakeEntity.Visible = false;

            //Assert
            Assert.True(eventRaised);
        }


        [Fact]
        public void Visible_WhenSettingValue_InvokesOnShowEvent()
        {
            //Arrange
            var fakeEntity = new FakeEntity(CreateTexture(), Vector.Zero)
            {
                Visible = false
            };
            var eventRaised = false;
            fakeEntity.OnShow += (sender, e) => { eventRaised = true; };

            //Act
            fakeEntity.Visible = true;

            //Assert
            Assert.True(eventRaised);
        }


        [Fact]
        public void Visible_WhenGoingFromInvisibleToVisible_InvokesOnShowEvent()
        {
            //Arrange
            var fakeEntity = new FakeEntity(CreateTexture(), Vector.Zero)
            {
                Visible = false
            };
            var eventRaised = false;
            fakeEntity.OnShow += (sender, e) => { eventRaised = true; };

            //Act
            fakeEntity.Visible = true;

            //Assert
            Assert.True(eventRaised);
        }


        [Fact]
        public void Visible_WhenGoingFromInvisibleToVisible_DoesNotInvokesOnShowEvent()
        {
            //Arrange
            var fakeEntity = new FakeEntity(CreateTexture(), Vector.Zero)
            {
                Visible = true
            };
            var eventRaised = false;
            fakeEntity.OnShow += (sender, e) => { eventRaised = true; };

            //Act
            fakeEntity.Visible = false;

            //Assert
            Assert.False(eventRaised);
        }


        [Fact]
        public void Bounds_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var fakeEntity = new FakeEntity(CreateTexture(), new Vector(111, 222))
            {
                Body = new PhysicsBody(It.IsAny<Vector[]>(), It.IsAny<Vector>())
            };
            fakeEntity.Initialize();

            var expected = new Rect(0, 0, 100, 100);

            //Act
            var actual = fakeEntity.Bounds;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void BoundsWidth_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var fakeEntity = new FakeEntity(CreateTexture(), It.IsAny<Vector>())
            {
                Body = new PhysicsBody(It.IsAny<Vector[]>(), It.IsAny<Vector>())
            };

            fakeEntity.Initialize();
            var expected = 100;

            //Act
            var actual = fakeEntity.BoundsWidth;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void BoundsWidth_WhenGettingValueWithNullBody_ReturnsZero()
        {
            //Arrange
            float[] nums = null;
            _mockPhysicsBody.Setup(m => m.XVertices).Returns(nums);

            var fakeEntity = new FakeEntity(CreateTexture(), It.IsAny<Vector>())
            {
                Body = new PhysicsBody(It.IsAny<Vector[]>(), It.IsAny<Vector>())
            };

            fakeEntity.Initialize();
            var expected = 0;

            //Act
            var actual = fakeEntity.BoundsWidth;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void BoundsHeight_WhenGettingValueWithNullBody_ReturnsZero()
        {
            //Arrange
            float[] nums = null;
            _mockPhysicsBody.Setup(m => m.YVertices).Returns(nums);

            var fakeEntity = new FakeEntity(CreateTexture(), It.IsAny<Vector>())
            {
                Body = new PhysicsBody(It.IsAny<Vector[]>(), It.IsAny<Vector>())
            };

            fakeEntity.Initialize();
            var expected = 0;

            //Act
            var actual = fakeEntity.BoundsHeight;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void BoundsHeight_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var fakeEntity = new FakeEntity(CreateTexture(), It.IsAny<Vector>())
            {
                Body = new PhysicsBody(It.IsAny<Vector[]>(), It.IsAny<Vector>())
            };

            fakeEntity.Initialize();
            var expected = 100;

            //Act
            var actual = fakeEntity.BoundsHeight;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void BoundsHalfWidth_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var fakeEntity = new FakeEntity(CreateTexture(), It.IsAny<Vector>())
            {
                Body = new PhysicsBody(It.IsAny<Vector[]>(), It.IsAny<Vector>())
            };

            fakeEntity.Initialize();
            var expected = 50;

            //Act
            var actual = fakeEntity.BoundsHalfWidth;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void BoundsHalfHeight_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var fakeEntity = new FakeEntity(CreateTexture(), It.IsAny<Vector>())
            {
                Body = new PhysicsBody(It.IsAny<Vector[]>(), It.IsAny<Vector>())
            };

            fakeEntity.Initialize();
            var expected = 50f;

            //Act
            var actual = fakeEntity.BoundsHalfHeight;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Texture_WhenSettingAndeGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            Vector[] vertices = new[] { Vector.Zero };
            var fakeEntity = new FakeEntity(vertices, It.IsAny<Vector>());

            //Act
            fakeEntity.Texture = CreateTexture();
            var actual = fakeEntity.Texture;

            //Assert
            Assert.NotNull(actual);
        }


        [Fact]
        public void DebugDrawEnabled_WhenSettingToTrue_ReturnsTrue()
        {
            //Arrange
            var mockPhysicsBody = new Mock<IPhysicsBody>();
            var mockEnginePluginLib = new Mock<IPluginLibrary>();

            var mockDebugDraw = new Mock<IDebugDraw>();

            mockEnginePluginLib.Setup(m => m.LoadPlugin<IDebugDraw>()).Returns(() =>
            {
                return mockDebugDraw.Object;
            });

            Vector[] vertices = new[] { Vector.Zero };
            var fakeEntity = new FakeEntity(vertices, It.IsAny<Vector>());
            var expected = true;

            //Act
            fakeEntity.DebugDrawEnabled = true;
            var actual = fakeEntity.DebugDrawEnabled;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void DebugDrawEnabled_WhenSettingToFalse_ReturnsFalse()
        {
            //Arrange
            var mockPhysicsBody = new Mock<IPhysicsBody>();
            var mockEnginePluginLib = new Mock<IPluginLibrary>();

            var mockDebugDraw = new Mock<IDebugDraw>();

            mockEnginePluginLib.Setup(m => m.LoadPlugin<IDebugDraw>()).Returns(() =>
            {
                return mockDebugDraw.Object;
            });

            Vector[] vertices = new[] { Vector.Zero };
            var fakeEntity = new FakeEntity(vertices, It.IsAny<Vector>());
            var expected = false;

            //Act
            fakeEntity.DebugDrawEnabled = false;
            var actual = fakeEntity.DebugDrawEnabled;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Behaviors_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockBehavior = new Mock<IBehavior>();
            var mockPhysicsBody = new Mock<IPhysicsBody>();

            var fakeEntity = new FakeEntity(CreateTexture(), It.IsAny<Vector>());
            var expected = new EntityBehaviors
            {
                mockBehavior.Object
            };

            //Act
            fakeEntity.Behaviors = new EntityBehaviors() { mockBehavior.Object };
            var actual = fakeEntity.Behaviors;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Position_WhenSettingValueAfterInitalized_ReturnsCorrectValue()
        {
            //Arrange
            _mockPhysicsBody.SetupProperty(m => m.X);
            _mockPhysicsBody.SetupProperty(m => m.Y);

            var fakeEntity = new FakeEntity(CreateTexture(), It.IsAny<Vector>())
            {
                Body = new PhysicsBody(It.IsAny<Vector[]>(), It.IsAny<Vector>())
            };
            fakeEntity.Initialize();

            var expected = new Vector(123, 456);

            //Act
            fakeEntity.Position = new Vector(123, 456);
            var actual = fakeEntity.Position;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Position_WhenSettingValueBeforeInitialized_ReturnsCorrectValue()
        {
            //Arrange
            var fakeEntity = new FakeEntity(CreateTexture(), It.IsAny<Vector>());
            var expected = new Vector(123, 456);

            //Act
            fakeEntity.Position = new Vector(123, 456);
            var actual = fakeEntity.Position;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Vertices_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var entity = new FakeEntity(true);
            var expected = new Vector[]
            {
                new Vector(11, 22),
                new Vector(33, 44),
                new Vector(55, 66)
            };

            //Act
            entity.Vertices = expected;
            var actual = entity.Vertices;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Vertices_WhenSettingValueAfterInit_ThrowsException()
        {
            //Arrange
            var entity = new FakeEntity(true)
            {
                Body = new PhysicsBody(It.IsAny<Vector[]>(), It.IsAny<Vector>())
            };
            entity.Initialize();

            var expected = new Vector[]
            {
                new Vector(11, 22),
                new Vector(33, 44),
                new Vector(55, 66)
            };

            //Act/Assert
            Assert.Throws<EntityAlreadyInitializedException>(() => entity.Vertices = expected);
        }


        [Fact]
        public void Vertices_WhenGettingValueBeforeInit_ReturnsCorrectVallue()
        {
            //Arrange
            var vertices = new Vector[]
            {
                new Vector(11, 22),
                new Vector(33, 44),
                new Vector(55, 66)
            };

            var entity = new FakeEntity(vertices, It.IsAny<Vector>());
            var expected = new Vector[]
            {
                new Vector(11, 22),
                new Vector(33, 44),
                new Vector(55, 66)
            };

            //Act
            var actual = entity.Vertices;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Vertices_WhenGettingValueBeforeAfterInit_ReturnsCorrectVallue()
        {
            //Arrange
            var vertices = new Vector[]
            {
                new Vector(-50, -50),
                new Vector(50, -50),
                new Vector(50, 50),
                new Vector(-50, 50)
            };

            var entity = new FakeEntity(vertices, It.IsAny<Vector>())
            {
                Body = new PhysicsBody(It.IsAny<Vector[]>(), It.IsAny<Vector>())
            };

            var expected = new Vector[]
            {
                new Vector(-50, -50),
                new Vector(50, -50),
                new Vector(50, 50),
                new Vector(-50, 50)
            };
            entity.Initialize();

            //Act
            var actual = entity.Vertices;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Method Tests
        [Fact]
        public void OnUpdate_WhenInvoked_UpdatesBehaviors()
        {
            //Arrange
            var mockBehavior = new Mock<IBehavior>();
            var fakeEntity = new FakeEntity(CreateTexture(), It.IsAny<Vector>());
            fakeEntity.Behaviors.Add(mockBehavior.Object);
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };

            //Act
            fakeEntity.Update(engineTime);

            //Assert
            mockBehavior.Verify(m => m.Update(It.IsAny<EngineTime>()), Times.Once());
        }


        [Fact]
        public void Initialize_WhenInvokingWithNullVertices_ThrowsException()
        {
            //Arrange
            Vector[] nullVertices = null;
            var entity = new FakeEntity(nullVertices, It.IsAny<Vector>());

            //Act/Assert
            Assert.Throws<MissingVerticesException>(() => entity.Initialize());
        }


        [Fact]
        public void LoadContent_WhenInvoked_SetsContentLoadedPropToTrue()
        {
            //Arange
            var entity = new FakeEntity(true);
            var expected = true;

            //Act
            entity.LoadContent(_contentLoader);
            var actual = entity.ContentLoaded;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Public Methods
        public void Dispose()
        {
            _mockPhysicsBody = null;
            _mockDebugDraw = null;
            _contentLoader = null;
            _mockEnginePluginLib = null;
            _mockPhysicsPluginLib = null;
            _plugins = null;

            CorePluginSystem.ClearPlugins();
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
