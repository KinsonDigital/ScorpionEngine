using Moq;
using NUnit.Framework;
using KDScorpionCore.Content;
using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;

namespace KDScorpionCore.Tests.Content
{
    [TestFixture]
    public class ContentLoaderTests
    {
        #region Fields
        private IContentLoader _contentLoader;
        private Mock<IContentLoader> _mockCoreContentLoader = new Mock<IContentLoader>();
        #endregion


        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvoking_SetsInternalLoader()
        {
            //Arrange
            var loader = new ContentLoader(_contentLoader);
            var expected = "RootDir";

            //Act
            loader.ContentRootDirectory = "RootDir";
            var actual = loader.ContentRootDirectory;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Prop Tests
        [Test]
        public void GamePath_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var loader = new ContentLoader(_contentLoader);
            var expected = "GamePath";

            //Act
            var actual = loader.GamePath;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ContentRootDirectory_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var loader = new ContentLoader(_contentLoader);
            var expected = "MyRootDir";

            //Act
            loader.ContentRootDirectory = "MyRootDir";
            var actual = loader.ContentRootDirectory;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Public Methods
        [Test]
        public void LoadTexture_WhenInvoked_ReturnsTexture()
        {
            //Arrange
            var loader = new ContentLoader(_contentLoader);

            //Act
            var actual = loader.LoadTexture("TextureName");

            //Assert
            Assert.NotNull(actual);
            _mockCoreContentLoader.Verify(m => m.LoadTexture<ITexture>(It.IsAny<string>()), Times.Once());
        }


        [Test]
        public void LoadText_WhenInvoked_ReturnsText()
        {
            //Arrange
            var loader = new ContentLoader(_contentLoader);

            //Act
            var actual = loader.LoadText("TextName");

            //Assert
            Assert.NotNull(actual);
            _mockCoreContentLoader.Verify(m => m.LoadText<IText>(It.IsAny<string>()), Times.Once());
        }
        #endregion


        #region Public Methods
        [SetUp]
        public void Setup()
        {
            var mockTexture = new Mock<ITexture>();
            var mockText = new Mock<IText>();

            _mockCoreContentLoader = new Mock<IContentLoader>();
            _mockCoreContentLoader.SetupProperty(m => m.ContentRootDirectory);
            _mockCoreContentLoader.SetupGet(m => m.GamePath).Returns("GamePath");
            _mockCoreContentLoader.Setup(m => m.LoadTexture<ITexture>(It.IsAny<string>())).Returns(() =>
            {
                return mockTexture.Object;
            });
            _mockCoreContentLoader.Setup(m => m.LoadText<IText>(It.IsAny<string>())).Returns(() =>
            {
                return mockText.Object;
            });

            _contentLoader = _mockCoreContentLoader.Object;
        }


        [TearDown]
        public void TearDown()
        {
            _contentLoader = null;
        }
        #endregion
    }
}
