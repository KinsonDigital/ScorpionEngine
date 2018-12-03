using Moq;
using NUnit.Framework;
using ScorpionCore;
using ScorpionCore.Graphics;
using ScorpionCore.Plugins;
using ScorpionEngine.Behaviors;
using ScorpionEngine.Exceptions;
using ScorpionEngine.Tests.Fakes;
using System;

namespace ScorpionEngine.Tests.Entities
{
    [TestFixture]
    public class EntityTests
    {
        private Mock<IDebugDraw> _mockDebugDraw;
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
            var mockPhysicsBody = new Mock<IPhysicsBody>();
            mockPhysicsBody.SetupProperty(m => m.X);
            mockPhysicsBody.SetupProperty(m => m.Y);

            Helpers.SetupPluginLib<IPhysicsBody, object[]>(mockPhysicsBody, PluginLibType.Physics);

            var mockTexture = new Mock<ITexture>();
            mockTexture.SetupGet(m => m.Width).Returns(100);
            mockTexture.SetupGet(m => m.Height).Returns(50);

            var texture = new Texture() { InternalTexture = mockTexture.Object };
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
            var mockPhysicsBody = new Mock<IPhysicsBody>();
            mockPhysicsBody.SetupProperty(m => m.X);
            mockPhysicsBody.SetupProperty(m => m.Y);

            var mockPluginLib = new Mock<IPluginLibrary>();
            mockPluginLib.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParams) =>
            {
                return new FakePhysicsBody((float[])ctorParams[0], (float[])ctorParams[1], (float)ctorParams[2], (float)ctorParams[3]);
            });

            PluginSystem.LoadPhysicsPluginLibrary(mockPluginLib.Object);

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
            var mockPhysicsBody = new Mock<IPhysicsBody>();
            mockPhysicsBody.SetupProperty(m => m.X);
            mockPhysicsBody.SetupProperty(m => m.Y);

            var mockTexture = new Mock<ITexture>();
            mockTexture.SetupGet(m => m.Width).Returns(100);
            mockTexture.SetupGet(m => m.Height).Returns(50);

            var mockPluginLib = new Mock<IPluginLibrary>();
            mockPluginLib.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParams) =>
            {
                return new FakePhysicsBody((float[])ctorParams[0], (float[])ctorParams[1], (float)ctorParams[2], (float)ctorParams[3]);
            });

            PluginSystem.LoadPhysicsPluginLibrary(mockPluginLib.Object);

            var texture = new Texture() { InternalTexture = mockTexture.Object };

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
            var expected = new Rect(111, 222, 100, 50);

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
            var mockPhysicsBody = new Mock<IPhysicsBody>();
            var mockPhysicsPluginLib = new Mock<IPluginLibrary>();
            float[] nums = null;
            mockPhysicsBody.Setup(m => m.XVertices).Returns(nums);
            mockPhysicsBody.Setup(m => m.YVertices).Returns(nums);

            mockPhysicsPluginLib.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParams) =>
            {
                return new FakePhysicsBody(nums, nums);
            });

            PluginSystem.LoadPhysicsPluginLibrary(mockPhysicsPluginLib.Object);


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
            var mockPhysicsBody = new Mock<IPhysicsBody>();
            var mockPhysicsPluginLib = new Mock<IPluginLibrary>();
            float[] nums = null;
            mockPhysicsBody.Setup(m => m.XVertices).Returns(nums);
            mockPhysicsBody.Setup(m => m.YVertices).Returns(nums);

            mockPhysicsPluginLib.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParams) =>
            {
                return new FakePhysicsBody(nums, nums);
            });

            PluginSystem.LoadPhysicsPluginLibrary(mockPhysicsPluginLib.Object);

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
            var expected = 50;

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
            var expected = 25f;

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
            var mockPhysicsPluginLib = new Mock<IPluginLibrary>();
            var mockEnginePluginLib = new Mock<IPluginLibrary>();

            var mockDebugDraw = new Mock<IDebugDraw>();

            mockEnginePluginLib.Setup(m => m.LoadPlugin<IDebugDraw>()).Returns(() =>
            {
                return mockDebugDraw.Object;
            });

            mockPhysicsPluginLib.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParams) =>
            {
                return new FakePhysicsBody((float[])ctorParams[0], (float[])ctorParams[1], (float)ctorParams[2], (float)ctorParams[3]);
            });

            PluginSystem.LoadPhysicsPluginLibrary(mockPhysicsPluginLib.Object);
            PluginSystem.LoadEnginePluginLibrary(mockEnginePluginLib.Object);

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
            var mockPhysicsPluginLib = new Mock<IPluginLibrary>();
            var mockEnginePluginLib = new Mock<IPluginLibrary>();

            var mockDebugDraw = new Mock<IDebugDraw>();

            mockEnginePluginLib.Setup(m => m.LoadPlugin<IDebugDraw>()).Returns(() =>
            {
                return mockDebugDraw.Object;
            });

            mockPhysicsPluginLib.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParams) =>
            {
                return new FakePhysicsBody((float[])ctorParams[0], (float[])ctorParams[1], (float)ctorParams[2], (float)ctorParams[3]);
            });

            PluginSystem.LoadPhysicsPluginLibrary(mockPhysicsPluginLib.Object);
            PluginSystem.LoadEnginePluginLibrary(mockEnginePluginLib.Object);

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
            var mockPhysicsPluginLib = new Mock<IPluginLibrary>();

            mockPhysicsPluginLib.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParams) =>
            {
                return new FakePhysicsBody((float[])ctorParams[0], (float[])ctorParams[1], (float)ctorParams[2], (float)ctorParams[3]);
            });

            PluginSystem.LoadPhysicsPluginLibrary(mockPhysicsPluginLib.Object);

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
            var mockPhysicsBody = new Mock<IPhysicsBody>();
            var mockPhysicsPluginLib = new Mock<IPluginLibrary>();

            mockPhysicsPluginLib.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParams) =>
            {
                return new FakePhysicsBody((float[])ctorParams[0], (float[])ctorParams[1], (float)ctorParams[2], (float)ctorParams[3]);
            });

            PluginSystem.LoadPhysicsPluginLibrary(mockPhysicsPluginLib.Object);

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
            var mockPhysicsPluginLib = new Mock<IPluginLibrary>();

            mockPhysicsPluginLib.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParams) =>
            {
                return new FakePhysicsBody((float[])ctorParams[0], (float[])ctorParams[1], (float)ctorParams[2], (float)ctorParams[3]);
            });

            PluginSystem.LoadPhysicsPluginLibrary(mockPhysicsPluginLib.Object);

            var texture = CreateTexture();
            var fakeEntity = new FakeEntity(texture, Vector.Zero);
            var expected = new Vector(123, 456);

            //Act
            fakeEntity.Position = new Vector(123, 456);
            var actual = fakeEntity.Position;

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
            _mockDebugDraw.Verify(m => m.Draw(mockRenderer.Object, It.IsAny<FakePhysicsBody>()), Times.Once());
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
        #endregion


        #region Public Methods
        [SetUp]
        public void Setup()
        {
            _mockDebugDraw = new Mock<IDebugDraw>();
            _mockDebugDraw.Setup(m => m.Draw(It.IsAny<IRenderer>(), It.IsAny<IPhysicsBody>()));

            var mockEnginePluginLib = new Mock<IPluginLibrary>();
            mockEnginePluginLib.Setup(m => m.LoadPlugin<IDebugDraw>()).Returns(() =>
            {
                return _mockDebugDraw.Object;
            });

            var mockPhysicsBody = new Mock<IPhysicsBody>();
            var mockPhysicsPluginLib = new Mock<IPluginLibrary>();

            mockPhysicsPluginLib.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParams) =>
            {
                return new FakePhysicsBody((float[])ctorParams[0], (float[])ctorParams[1], (float)ctorParams[2], (float)ctorParams[3]);
            });

            PluginSystem.LoadEnginePluginLibrary(mockEnginePluginLib.Object);
            PluginSystem.LoadPhysicsPluginLibrary(mockPhysicsPluginLib.Object);
        }


        [TearDown]
        public void TearDown()
        {
            PluginSystem.ClearPlugins();
        }
        #endregion


        #region Private Methods
        private static Texture CreateTexture()
        {
            var mockTexture = new Mock<ITexture>();
            mockTexture.SetupGet(m => m.Width).Returns(100);
            mockTexture.SetupGet(m => m.Height).Returns(50);


            return new Texture() { InternalTexture = mockTexture.Object };
        }
        #endregion
    }
}
