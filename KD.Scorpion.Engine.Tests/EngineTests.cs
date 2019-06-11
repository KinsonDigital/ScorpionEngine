using Moq;
using NUnit.Framework;
using KDScorpionCore;
using KDScorpionCore.Content;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Scene;
using KDScorpionEngineTests.Fakes;
using System;
using KDScorpionEngine;
using PluginSystem;

namespace KDScorpionEngineTests
{
    [TestFixture]
    public class EngineTests
    {
        #region Fields
        private ContentLoader _contentLoader;
        private Mock<IEngineCore> _mockEngineCore;
        private Mock<IPluginFactory> _mockPluginFactory;
        private Mock<IPluginLibrary> _mockPhysicsPluginLib;
        #endregion


        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvoking_SetsContentLoader()
        {
            //Arrange
            var engine = new Engine(_mockPluginFactory.Object);

            //Act
            var actual = engine.ContentLoader;

            //Assert
            Assert.NotNull(actual);
        }


        [Test]
        public void Ctor_WhenInvoking_SetsEngineCore()
        {
            //Arrange
            var engine = new Engine(_mockPluginFactory.Object);
            var expected = true;

            //Act
            var actual = engine.Running;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvoking_SetsUpOnIntializeEvent()
        {
            //Arrange
            var engine = new Engine(_mockPluginFactory.Object);
            var expected = true;

            //Act
            var actual = engine.Running;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvoking_SetsUpEngineCoreEvents()
        {
            //Arrange
            var mockRenderer = new Mock<IRenderer>();
            var fakeEngineCore = new FakeEngineCore
            {
                Renderer = mockRenderer.Object
            };

            var mockKeyboard = new Mock<IKeyboard>();
            var mockPluginFactory = new Mock<IPluginFactory>();
            mockPluginFactory.Setup(m => m.CreateEngineCore()).Returns(fakeEngineCore);
            mockPluginFactory.Setup(m => m.CreateKeyboard()).Returns(mockKeyboard.Object);
            mockPluginFactory.Setup(m => m.CreateRenderer()).Returns(mockRenderer.Object);

            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.Id).Returns(10);

            Plugins.LoadPluginFactory(mockPluginFactory.Object);

            var manager = new SceneManager(_contentLoader)
            {
                mockScene.Object
            };
            manager.SetCurrentSceneID(10);

            var engine = new FakeEngine(mockPluginFactory.Object)
            {
                SceneManager = manager
            };
            var expected = true;

            //Act
            fakeEngineCore.InvokeAllEvents();
            var actualInitInvoked = engine.InitInvoked;
            var actualLoadInvoked = engine.LoadContentInvoked;
            var actualUpdateInvoked = engine.UpdateInvoked;
            var actualRenderInvoked = engine.RenderInvoked;

            //Assert
            Assert.AreEqual(expected, actualInitInvoked);
            Assert.AreEqual(expected, actualLoadInvoked);
            Assert.AreEqual(expected, actualUpdateInvoked);
            Assert.AreEqual(expected, actualRenderInvoked);
        }
        #endregion


        #region Prop Tests
        [Test]
        public void SceneManager_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var engine = new Engine(_mockPluginFactory.Object);
            var manager = new SceneManager(_contentLoader);
            var expected = manager;

            //Act
            engine.SceneManager = manager;
            var actual = engine.SceneManager;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ContentLoader_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var engine = new Engine(_mockPluginFactory.Object);
            var expected = _contentLoader;

            //Act
            engine.ContentLoader = _contentLoader;
            var actual = engine.ContentLoader;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Running_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var engine = new Engine(_mockPluginFactory.Object);
            var expected = true;

            //Act
            var actual = engine.Running;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void CurrentFPS_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var engine = new Engine(_mockPluginFactory.Object);
            var expected = 62.5f;

            //Act
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            engine.Update(engineTime);
            var actual = Engine.CurrentFPS;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void WindowWidth_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var engine = new Engine(_mockPluginFactory.Object);
            var expected = 123;

            //Act
            Engine.WindowWidth = 123;
            var actual = Engine.WindowWidth;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void WindowHeight_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var engine = new Engine(_mockPluginFactory.Object);
            var expected = 123;

            //Act
            Engine.WindowHeight = 123;
            var actual = Engine.WindowHeight;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Method Tests
        [Test]
        public void Start_WhenInvoked_InvokesEngineCoreStart()
        {
            //Arrange
            var engine = new Engine(_mockPluginFactory.Object);

            //Act
            engine.Start();

            //Assert
            _mockEngineCore.Verify(m => m.Start(), Times.Once());
        }


        [Test]
        public void Start_WhenInvokedWithNullEngineCore_DoesNotThrowException()
        {
            //Arrange
            var engine = new Engine(_mockPluginFactory.Object);

            engine.SetFieldNull("_engineCore");

            //Act/Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() => engine.Start());
        }


        [Test]
        public void Stop_WhenInvoked_InvokesEngineCoreStop()
        {
            //Arrange
            var engine = new Engine(_mockPluginFactory.Object);

            //Act
            engine.Stop();

            //Assert
            _mockEngineCore.Verify(m => m.Stop(), Times.Once());
        }


        [Test]
        public void Stop_WhenInvokedWithNullEngineCore_DoesNotThrowException()
        {
            //Arrange
            var engine = new Engine(_mockPluginFactory.Object);

            engine.SetFieldNull("_engineCore");

            //Act/Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() => engine.Stop());
        }


        [Test]
        public void Update_WhenInvokingWhileRunning_SetsCurrentFPSProp()
        {
            //Arrange
            var engine = new Engine(_mockPluginFactory.Object);
            var expected = 62.5f;

            //Act
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            engine.Update(engineTime);
            var actual = Engine.CurrentFPS;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Update_WhenInvokingWhileNotRunning_DoesNotSetCurrentFPSProp()
        {
            //Arrange
            PauseEngineCore();

            var engine = new Engine(_mockPluginFactory.Object);
            var expected = 0f;
            Engine.CurrentFPS = 0;

            //Act
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            engine.Update(engineTime);
            var actual = Engine.CurrentFPS;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Dispose_WhenInvoking_DisposesEngineCore()
        {
            //Arrange
            TearDown();

            var fakeEngineCore = new FakeEngineCore();
            var mockPluginFactory = new Mock<IPluginFactory>();
            mockPluginFactory.Setup(m => m.CreateEngineCore()).Returns(fakeEngineCore);

            Plugins.LoadPluginFactory(mockPluginFactory.Object);

            var engine = new FakeEngine(mockPluginFactory.Object);
            var expected = true;

            //Act
            engine.Dispose();
            var actual = fakeEngineCore.DisposeInvoked;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Public Methods
        [SetUp]
        public void Setup()
        {
            var mockContentLoader = new Mock<IContentLoader>();
            _contentLoader = new ContentLoader(mockContentLoader.Object);

            _mockEngineCore = new Mock<IEngineCore>();
            _mockEngineCore.Setup(m => m.IsRunning()).Returns(true);
            _mockEngineCore.SetupProperty(m => m.WindowWidth);
            _mockEngineCore.SetupProperty(m => m.WindowHeight);

            var mockKeyboard = new Mock<IKeyboard>();

            _mockPluginFactory = new Mock<IPluginFactory>();
            _mockPluginFactory.Setup(m => m.CreateContentLoader()).Returns(mockContentLoader.Object);
            _mockPluginFactory.Setup(m => m.CreateEngineCore()).Returns(_mockEngineCore.Object);
            _mockPluginFactory.Setup(m => m.CreateKeyboard()).Returns(mockKeyboard.Object);

            Plugins.LoadPluginFactory(_mockPluginFactory.Object);
        }


        [TearDown]
        public void TearDown()
        {
            Plugins.UnloadPluginFactory();

            _contentLoader = null;
            _mockPluginFactory = null;
            _mockPhysicsPluginLib = null;
        }
        #endregion


        #region Private Methods
        private void PauseEngineCore()
        {
            _mockEngineCore.Setup(m => m.IsRunning()).Returns(false);
        }
        #endregion
    }
}
