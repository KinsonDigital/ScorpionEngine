using Moq;
using NUnit.Framework;
using ParticleMaker.Services;
using System.IO;
using System.Reflection;

namespace ParticleMaker.Tests.Services
{
    [TestFixture]
    public class ContentDirectoryServiceTests
    {
        #region Fields
        private string _createdTestDir;
        private const string TEST_DIR = @"C:\particle-maker-unit-testing";
        #endregion


        #region Constructor Tests
        [Test]
        public void Ctor_WithContentRootDirNotExisting_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            var mockFileService = new Mock<IFileService>();

            var dirToDelete = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Content";

            //Act
            var service = new ContentDirectoryService(mockDirService.Object, mockFileService.Object);

            //Assert
            mockDirService.Verify(m => m.Exists(It.IsAny<string>()), Times.Once());
            mockDirService.Verify(m => m.Create(It.IsAny<string>()), Times.Once());
        }
        #endregion


        #region Prop Tests
        [Test]
        public void ContentRootDirectory_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            var mockFileService = new Mock<IFileService>();

            var service = new ContentDirectoryService(mockDirService.Object, mockFileService.Object);
            _createdTestDir = service.ContentRootDirectory;

            var expected = TEST_DIR;

            //Act
            service.ContentRootDirectory = TEST_DIR;
            var actual = service.ContentRootDirectory;
            
            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Method Tests
        [Test]
        public void ContentItemExists_WhenInvokedWithExistingFile_ReturnsTrue()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var service = new ContentDirectoryService(mockDirService.Object, mockFileService.Object);

            var expected = true;
            
            //Act
            var actual = service.ContentItemExists("test-file");

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
