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
        private Mock<IPluginLibrary> _mockPluginLibrary;
        #endregion


        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvoking_SetsContentLoader()
        {
            //Arrange
            var engine = new Engine(_mockPluginLibrary.Object);

            //Act
            var actual = engine.ContentLoader;

            //Assert
            Assert.NotNull(actual);
        }
        #endregion


        #region Prop Tests
        [Test]
        public void SceneManager_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var engine = new Engine(_mockPluginLibrary.Object);
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
            var engine = new Engine(_mockPluginLibrary.Object);
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
            var engine = new Engine(_mockPluginLibrary.Object);
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
            var engine = new Engine(_mockPluginLibrary.Object);
            var expected = 62.5f;

            //Act
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            engine.Start();
            engine.Update(engineTime);
            var actual = Engine.CurrentFPS;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void WindowWidth_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
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
            var engine = new Engine(_mockPluginLibrary.Object);

            //Act
            engine.Start();

            //Assert
            _mockEngineCore.Verify(m => m.Start(), Times.Once());
        }


        [Test]
        public void Start_WhenInvokedWithNullEngineCore_DoesNotThrowException()
        {
            //Arrange
            var engine = new Engine(_mockPluginLibrary.Object);

            engine.SetFieldNull("_engineCore");

            //Act/Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() => engine.Start());
        }


        [Test]
        public void Stop_WhenInvoked_InvokesEngineCoreStop()
        {
            //Arrange
            var engine = new Engine(_mockPluginLibrary.Object);

            //Act
            engine.Stop();

            //Assert
            _mockEngineCore.Verify(m => m.Stop(), Times.Once());
        }


        [Test]
        public void Stop_WhenInvokedWithNullEngineCore_DoesNotThrowException()
        {
            //Arrange
            var engine = new Engine(_mockPluginLibrary.Object);

            engine.SetFieldNull("_engineCore");

            //Act/Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() => engine.Stop());
        }


        [Test]
        public void Update_WhenInvokingWhileRunning_SetsCurrentFPSProp()
        {
            //Arrange
            var engine = new Engine(_mockPluginLibrary.Object);
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

            var engine = new Engine(_mockPluginLibrary.Object);
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
            var mockPluginLibrary = new Mock<IPluginLibrary>();
            mockPluginLibrary.Setup(m => m.LoadPlugin<IEngineCore>()).Returns(fakeEngineCore);

            Plugins.EnginePlugins = mockPluginLibrary.Object;

            var engine = new FakeEngine(mockPluginLibrary.Object);
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

            _mockPluginLibrary = new Mock<IPluginLibrary>();
            _mockPluginLibrary.Setup(m => m.LoadPlugin<IContentLoader>()).Returns(mockContentLoader.Object);
            _mockPluginLibrary.Setup(m => m.LoadPlugin<IEngineCore>()).Returns(_mockEngineCore.Object);
            _mockPluginLibrary.Setup(m => m.LoadPlugin<IKeyboard>()).Returns(mockKeyboard.Object);

            Plugins.EnginePlugins = _mockPluginLibrary.Object;
        }


        [TearDown]
        public void TearDown()
        {
            Plugins.EnginePlugins = null;
            _contentLoader = null;
            _mockPluginLibrary = null;
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
