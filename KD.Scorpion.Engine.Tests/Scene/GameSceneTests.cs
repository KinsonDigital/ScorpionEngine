using Moq;
using NUnit.Framework;
using KDScorpionCore;
using KDScorpionCore.Content;
using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Scene;
using KDScorpionEngine.Tests.Fakes;
using System;

namespace KDScorpionEngine.Tests.Scene
{
    [TestFixture]
    public class GameSceneTests
    {
        #region Fields
        private Mock<IPhysicsWorld> _mockPhysicsWorld;
        #endregion


        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvoking_CreatePhysicsWorld()
        {
            //Arrange
            var scene = new FakeGameScene(Vector.Zero);

            //Act
            var actual = GameScene.PhysicsWorld;

            //Assert
            Assert.NotNull(actual);
        }
        #endregion


        #region Prop Tests
        [Test]
        public void Name_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var scene = new FakeGameScene(Vector.Zero);
            var expected = "John Doe";

            //Act
            scene.Name = "John Doe";
            var actual = scene.Name;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ContentLoaded_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var scene = new FakeGameScene(Vector.Zero);
            var expected = true;

            //Act
            scene.ContentLoaded = true;
            var actual = scene.ContentLoaded;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void TimeManager_WhenGettingValue_NotNull()
        {
            //Arrange
            var scene = new FakeGameScene(Vector.Zero);

            //Act
            var actual = scene.TimeManager;

            //Assert
            Assert.NotNull(actual);
        }


        [Test]
        public void Initialized_WhenGettingValueAfterInitialized_ReturnsTrue()
        {
            //Arrange
            var scene = new FakeGameScene(Vector.Zero);
            var expected = true;

            //Act
            scene.Initialize();
            var actual = scene.Initialized;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Active_WhenGettingAndSettingValue_ReturnsTrue()
        {
            //Arrange
            var scene = new FakeGameScene(Vector.Zero);
            var expected = true;

            //Act
            scene.Active = true;
            var actual = scene.Active;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void IsRenderingScene_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var scene = new FakeGameScene(Vector.Zero);
            var expected = true;

            //Act
            scene.IsRenderingScene = true;
            var actual = scene.IsRenderingScene;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Id_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var scene = new FakeGameScene(Vector.Zero);
            var expected = 10;

            //Act
            scene.Id = 10;
            var actual = scene.Id;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Method Tests
        [Test]
        public void LoadContent_WhenInvoked_SetsContentLoadedToTrue()
        {
            //Arrange
            var mockCoreLoader = new Mock<IContentLoader>();
            var loader = new ContentLoader(mockCoreLoader.Object);
            var scene = new FakeGameScene(Vector.Zero);
            var expected = true;

            //Act
            scene.LoadContent(loader);
            var actual = scene.ContentLoaded;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void UnloadContent_WhenInvoked_SetsContentLoadedToFalse()
        {
            //Arrange
            var mockCoreLoader = new Mock<IContentLoader>();
            var loader = new ContentLoader(mockCoreLoader.Object);
            var scene = new FakeGameScene(Vector.Zero)
            {
                ContentLoaded = true
            };
            var expected = false;

            //Act
            scene.UnloadContent(loader);
            var actual = scene.ContentLoaded;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Update_WhenInvoking_InvokesTimeManagerUpdate()
        {
            //Arrange
            var mockTimeManager = new Mock<ITimeManager>();

            var scene = new FakeGameScene(Vector.Zero)
            {
                TimeManager = mockTimeManager.Object
            };
            
            //Act
            scene.Update(new EngineTime());

            //Assert
            mockTimeManager.Verify(m => m.Update(It.IsAny<EngineTime>()), Times.Once());
        }


        [Test]
        public void Update_WhenInvoking_InvokesPhyiscsWorldUpdate()
        {
            //Arrange
            var mockTimeManager = new Mock<ITimeManager>();

            var scene = new FakeGameScene(Vector.Zero);

            //Act
            scene.Update(new EngineTime());

            //Assert
            _mockPhysicsWorld.Verify(m => m.Update(It.IsAny<float>()), Times.Once());
        }


        [Test]
        public void Update_WhenInvoking_InvokesEntityUpdate()
        {
            //Arrange
            var entity = new FakeEntity(new Vector[0], Vector.Zero);
            var scene = new FakeGameScene(Vector.Zero);
            entity.Initialize();
            scene.AddEntity(entity);
            var expected = true;

            //Act
            scene.Update(new EngineTime());
            var actual = entity.UpdateInvoked;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Update_WhenInvokingWithNullTimeManager_DoesNotThrowException()
        {
            //Arrange
            var scene = new FakeGameScene(Vector.Zero)
            {
                TimeManager = null
            };

            //Act/Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() =>
            {
                scene.Update(new EngineTime());
            });
        }


        [Test]
        public void Render_WhenInvoking_InvokesAllEntityRenderMethods()
        {
            //Arrange
            var entityA = new FakeEntity(false);
            var entityB = new FakeEntity(false);

            var scene = new FakeGameScene(Vector.Zero);
            scene.AddEntity(entityA, false);
            scene.AddEntity(entityB, false);

            var mockCoreRenderer = new Mock<IRenderer>();
            var renderer = new Renderer(mockCoreRenderer.Object);

            var expected = true;

            //Act
            scene.Render(renderer);
            var actual = entityA.RenderInvoked && entityB.RenderInvoked;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Public Methods
        [SetUp]
        public void Setup()
        {
            _mockPhysicsWorld = new Mock<IPhysicsWorld>();

            var mockPhysicsPluginLib = new Mock<IPluginLibrary>();
            mockPhysicsPluginLib.Setup(m => m.LoadPlugin<IPhysicsWorld>(It.IsAny<object[]>())).Returns((object[] ctorParams) => {
                return _mockPhysicsWorld.Object;
            });

            PluginSystem.LoadPhysicsPluginLibrary(mockPhysicsPluginLib.Object);
        }


        [TearDown]
        public void TearDown()
        {
            PluginSystem.ClearPlugins();
            _mockPhysicsWorld = null;
        }
        #endregion
    }
}
