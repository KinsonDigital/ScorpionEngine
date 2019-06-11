using Moq;
using NUnit.Framework;
using KDScorpionCore;
using KDScorpionCore.Content;
using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Behaviors;
using KDScorpionEngine.Exceptions;
using KDScorpionEngineTests.Fakes;
using System;
using PluginSystem;

namespace KDScorpionEngineTests.Entities
{
    [TestFixture]
    public class EntityTests
    {
        private Mock<IPhysicsBody> _mockPhysicsBody;
        #region Fields
        private Mock<IDebugDraw> _mockDebugDraw;
        private ContentLoader _contentLoader;
        #endregion


        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvokingWithStaticBodyValue_ProperlySetsBodyAsStatic()
        {
            //Arrange
            var fakeEntity = new FakeEntity(true);
            var expected = true;

            //Act
            var actual = fakeEntity.IsStatic;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvokingWithPositionParam_ProperlySetsPositionProperty()
        {
            //Arrange
            var fakeEntity = new FakeEntity(new Vector(11, 22));
            var expected = new Vector(11, 22);

            //Act
            var actual = fakeEntity.Position;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvokingWithTexture_ProperlySetsUpObject()
        {
            //Arrange
            var mockTexture = new Mock<ITexture>();
            mockTexture.SetupGet(m => m.Width).Returns(100);
            mockTexture.SetupGet(m => m.Height).Returns(50);

            var texture = new Texture(mockTexture.Object);
            var fakeEntity = new FakeEntity(texture, Vector.Zero);
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


        [Test]
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
            Assert.AreEqual(expectedPosition, actualPosition);
        }


        [Test]
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
            var fakeEntity = new FakeEntity(texture, vertices, position);
            var actualPosition = fakeEntity.Position;
            var actualTexture = fakeEntity.Texture;

            //Assert
            Assert.NotNull(actualTexture);
            Assert.AreEqual(expectedPosition, actualPosition);
        }
        #endregion


        #region Prop Tests
        [Test]
        public void Behaviors_WhenCreatingEntity_PropertyInstantiated()
        {
            //Arrange
            var texture = CreateTexture();
            var fakeEntity = new FakeEntity(texture, Vector.Zero);

            //Act
            var actual = fakeEntity.Behaviors;

            //Assert
            Assert.NotNull(actual);
        }


        [Test]
        public void Visible_WhenGettingAndSettingValue_ValueProperlySet()
        {
            //Arrange
            var texture = CreateTexture();

            var fakeEntity = new FakeEntity(texture, Vector.Zero)
            {
                Visible = false
            };
            var expected = false;

            //Act
            var actual = fakeEntity.Visible;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Visible_WhenSettingValue_InvokesOnHideEvent()
        {
            //Arrange
            var texture = CreateTexture();

            var fakeEntity = new FakeEntity(texture, Vector.Zero);
            var eventRaised = false;
            fakeEntity.OnHide += (sender, e) => { eventRaised = true; };

            //Act
            fakeEntity.Visible = false;

            //Assert
            Assert.True(eventRaised);
        }


        [Test]
        public void Visible_WhenSettingValue_InvokesOnShowEvent()
        {
            //Arrange
            var texture = CreateTexture();

            var fakeEntity = new FakeEntity(texture, Vector.Zero)
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


        [Test]
        public void Visible_WhenGoingFromInvisibleToVisible_InvokesOnShowEvent()
        {
            //Arrange
            var texture = CreateTexture();

            var fakeEntity = new FakeEntity(texture, Vector.Zero)
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


        [Test]
        public void Visible_WhenGoingFromInvisibleToVisible_DoesNotInvokesOnShowEvent()
        {
            //Arrange
            var texture = CreateTexture();

            var fakeEntity = new FakeEntity(texture, Vector.Zero)
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


        [Test]
        public void Bounds_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var texture = CreateTexture();
            var fakeEntity = new FakeEntity(texture, new Vector(111, 222));
            fakeEntity.Initialize();
            var expected = new Rect(0, 0, 100, 100);

            //Act
            var actual = fakeEntity.Bounds;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void BoundsWidth_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var texture = CreateTexture();
            var fakeEntity = new FakeEntity(texture, Vector.Zero);
            fakeEntity.Initialize();
            var expected = 100;

            //Act
            var actual = fakeEntity.BoundsWidth;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void BoundsWidth_WhenGettingValueWithNullBody_ReturnsZero()
        {
            //Arrange
            float[] nums = null;
            _mockPhysicsBody.Setup(m => m.XVertices).Returns(nums);
            _mockPhysicsBody.Setup(m => m.YVertices).Returns(nums);

            var texture = CreateTexture();
            var fakeEntity = new FakeEntity(texture, Vector.Zero);
            fakeEntity.Initialize();
            var expected = 0;

            //Act
            var actual = fakeEntity.BoundsWidth;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void BoundsHeight_WhenGettingValueWithNullBody_ReturnsZero()
        {
            //Arrange
            float[] nums = null;
            _mockPhysicsBody.Setup(m => m.XVertices).Returns(nums);
            _mockPhysicsBody.Setup(m => m.YVertices).Returns(nums);

            var texture = CreateTexture();
            var fakeEntity = new FakeEntity(texture, Vector.Zero);
            fakeEntity.Initialize();
            var expected = 0;

            //Act
            var actual = fakeEntity.BoundsHeight;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void BoundsHeight_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var texture = CreateTexture();

            var fakeEntity = new FakeEntity(texture, Vector.Zero);
            fakeEntity.Initialize();
            var expected = 100;

            //Act
            var actual = fakeEntity.BoundsHeight;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void BoundsHalfWidth_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var texture = CreateTexture();

            var fakeEntity = new FakeEntity(texture, Vector.Zero);
            fakeEntity.Initialize();
            var expected = 50;

            //Act
            var actual = fakeEntity.BoundsHalfWidth;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void BoundsHalfHeight_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var texture = CreateTexture();
            var fakeEntity = new FakeEntity(texture, Vector.Zero);
            fakeEntity.Initialize();
            var expected = 50f;

            //Act
            var actual = fakeEntity.BoundsHalfHeight;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Texture_WhenSettingAndeGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var texture = CreateTexture();

            Vector[] vertices = new[] { Vector.Zero };
            var fakeEntity = new FakeEntity(vertices, Vector.Zero);

            //Act
            fakeEntity.Texture = texture;
            var actual = fakeEntity.Texture;

            //Assert
            Assert.NotNull(actual);
        }


        [Test]
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

            var texture = CreateTexture();

            Vector[] vertices = new[] { Vector.Zero };
            var fakeEntity = new FakeEntity(vertices, Vector.Zero);
            var expected = true;

            //Act
            fakeEntity.DebugDrawEnabled = true;
            var actual = fakeEntity.DebugDrawEnabled;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
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

            var texture = CreateTexture();

            Vector[] vertices = new[] { Vector.Zero };
            var fakeEntity = new FakeEntity(vertices, Vector.Zero);
            var expected = false;

            //Act
            fakeEntity.DebugDrawEnabled = false;
            var actual = fakeEntity.DebugDrawEnabled;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Behaviors_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockBehavior = new Mock<IBehavior>();
            var mockPhysicsBody = new Mock<IPhysicsBody>();

            var texture = CreateTexture();
            var fakeEntity = new FakeEntity(texture, Vector.Zero);
            var expected = new EntityBehaviors
            {
                mockBehavior.Object
            };

            //Act
            fakeEntity.Behaviors = new EntityBehaviors() { mockBehavior.Object };
            var actual = fakeEntity.Behaviors;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Position_WhenSettingValueAfterInitalized_ReturnsCorrectValue()
        {
            //Arrange
            _mockPhysicsBody.SetupProperty(m => m.X);
            _mockPhysicsBody.SetupProperty(m => m.Y);

            var texture = CreateTexture();
            var fakeEntity = new FakeEntity(texture, Vector.Zero);
            fakeEntity.Initialize();
            var expected = new Vector(123, 456);

            //Act
            fakeEntity.Position = new Vector(123, 456);
            var actual = fakeEntity.Position;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Position_WhenSettingValueBeforeInitialized_ReturnsCorrectValue()
        {
            //Arrange
            var mockPhysicsBody = new Mock<IPhysicsBody>();

            var texture = CreateTexture();
            var fakeEntity = new FakeEntity(texture, Vector.Zero);
            var expected = new Vector(123, 456);

            //Act
            fakeEntity.Position = new Vector(123, 456);
            var actual = fakeEntity.Position;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Vertices_WhenSettingValueAfterInit_ThrowsException()
        {
            //Arrange
            var entity = new FakeEntity(true);
            var expected = new Vector[]
            {
                new Vector(11, 22),
                new Vector(33, 44),
                new Vector(55, 66)
            };
            entity.Initialize();

            //Act/Assert
            Assert.Throws<EntityAlreadyInitializedException>(() => entity.Vertices = expected);
        }


        [Test]
        public void Vertices_WhenGettingValueBeforeInit_ReturnsCorrectVallue()
        {
            //Arrange
            var vertices = new Vector[]
            {
                new Vector(11, 22),
                new Vector(33, 44),
                new Vector(55, 66)
            };

            var entity = new FakeEntity(vertices, Vector.Zero);
            var expected = new Vector[]
            {
                new Vector(11, 22),
                new Vector(33, 44),
                new Vector(55, 66)
            };

            //Act
            var actual = entity.Vertices;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
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

            var entity = new FakeEntity(vertices, Vector.Zero);
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
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Method Tests
        [Test]
        public void OnUpdate_WhenInvoked_UpdatesBehaviors()
        {
            //Arrange
            var mockBehavior = new Mock<IBehavior>();
            var texture = CreateTexture();
            var fakeEntity = new FakeEntity(texture, Vector.Zero);
            fakeEntity.Behaviors.Add(mockBehavior.Object);
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };

            //Act
            fakeEntity.Update(engineTime);

            //Assert
            mockBehavior.Verify(m => m.Update(It.IsAny<EngineTime>()), Times.Once());
        }


        [Test]
        public void Render_WhenInvokedWhileVisible_InvokesRenderer()
        {
            //Arrange
            var mockRenderer = new Mock<IRenderer>();
            var renderer = new Renderer(mockRenderer.Object);
            var texture = CreateTexture();

            var fakeEntity = new FakeEntity(texture, Vector.Zero);
            fakeEntity.Initialize();

            //Act
            fakeEntity.Render(renderer);

            //Assert
            mockRenderer.Verify(m => m.Render(texture.InternalTexture, It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()), Times.Once());
        }


        [Test]
        public void Render_WhenInvokedWhileNotVisible_DoesNotInvokesRenderer()
        {
            //Arrange
            var mockRenderer = new Mock<IRenderer>();
            var renderer = new Renderer(mockRenderer.Object);
            var texture = CreateTexture();

            var fakeEntity = new FakeEntity(texture, Vector.Zero)
            {
                Visible = false
            };

            //Act
            fakeEntity.Render(renderer);

            //Assert
            mockRenderer.Verify(m => m.Render(texture.InternalTexture, It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()), Times.Never());
        }


        [Test]
        public void Render_WhenInvokedWhileWithNullTexture_DoesNotInvokesRenderer()
        {
            //Arrange
            var mockRenderer = new Mock<IRenderer>();
            var renderer = new Renderer(mockRenderer.Object);
            var texture = CreateTexture();
            var fakeEntity = new FakeEntity(texture, Vector.Zero)
            {
                Texture = null
            };

            //Act
            fakeEntity.Render(renderer);

            //Assert
            mockRenderer.Verify(m => m.Render(texture.InternalTexture, It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()), Times.Never());
        }


        [Test]
        public void Render_WhenInvoked_InvokesDebugDraw()
        {
            //Arrange
            var mockRenderer = new Mock<IRenderer>();
            var renderer = new Renderer(mockRenderer.Object);

            var texture = CreateTexture();

            var fakeEntity = new FakeEntity(texture, Vector.Zero)
            {
                DebugDrawEnabled = true
            };
            fakeEntity.Initialize();

            //Act
            fakeEntity.Render(renderer);

            //Assert
            _mockDebugDraw.Verify(m => m.Draw(mockRenderer.Object, It.IsAny<IPhysicsBody>()), Times.Once());
        }


        [Test]
        public void Render_WhenInvokedWithDebugDrawDisabled_DoesNotInvokesDebugDraw()
        {
            //Arrange
            var mockRenderer = new Mock<IRenderer>();
            var renderer = new Renderer(mockRenderer.Object);
            var texture = CreateTexture();

            var fakeEntity = new FakeEntity(texture, Vector.Zero);
            fakeEntity.Initialize();

            //Act
            fakeEntity.Render(renderer);

            //Assert
            _mockDebugDraw.Verify(m => m.Draw(mockRenderer.Object, It.IsAny<FakePhysicsBody>()), Times.Never());
        }


        [Test]
        public void Initialize_WhenInvokingWithNullVertices_ThrowsException()
        {
            //Arrange
            Vector[] nullVertices = null;
            var entity = new FakeEntity(nullVertices, Vector.Zero);

            //Act/Assert
            Assert.Throws<MissingVerticesException>(() => entity.Initialize());
        }


        [Test]
        public void LoadContent_WhenInvoked_SetsContentLoadedPropToTrue()
        {
            //Arange
            var entity = new FakeEntity(true);
            var expected = true;

            //Act
            entity.LoadContent(_contentLoader);
            var actual = entity.ContentLoaded;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Public Methods
        [SetUp]
        public void Setup()
        {
            _mockPhysicsBody = new Mock<IPhysicsBody>();
            _mockPhysicsBody.Setup(m => m.XVertices).Returns(new float[] { -50, 50, 50, -50 });
            _mockPhysicsBody.Setup(m => m.YVertices).Returns(new float[] { -50, -50, 50, 50 });

            _mockDebugDraw = new Mock<IDebugDraw>();
            _mockDebugDraw.Setup(m => m.Draw(It.IsAny<IRenderer>(), It.IsAny<IPhysicsBody>()));

            var mockContentLoader = new Mock<IContentLoader>();
            _contentLoader = new ContentLoader(mockContentLoader.Object);

            var mockPluginFactory = new Mock<IPluginFactory>();
            mockPluginFactory.Setup(m => m.CreatePhysicsBody(It.IsAny<object[]>())).Returns((object[] ctorParams) => _mockPhysicsBody.Object);

            mockPluginFactory.Setup(m => m.CreateDebugDraw()).Returns(() =>
            {
                return _mockDebugDraw.Object;
            });

            Plugins.LoadPluginFactory(mockPluginFactory.Object);
        }


        [TearDown]
        public void TearDown() => Plugins.UnloadPluginFactory();
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
