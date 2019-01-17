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
            var dirToDelete = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Content";
            DeleteFolderWithContents(dirToDelete);
            var expected = true;

            //Act
            var service = new ContentDirectoryService();
            var actual = Directory.Exists(dirToDelete);

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Prop Tests
        [Test]
        public void ContentRootDirectory_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var service = new ContentDirectoryService();
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
            var service = new ContentDirectoryService
            {
                ContentRootDirectory = TEST_DIR
            };
            var expected = true;
            
            //Act
            var actual = service.ContentItemExists("test-file");

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Public Methods
        [SetUp]
        public void Setup()
        {
            //First delete the test directory if it exists
            DeleteFolderWithContents(TEST_DIR);

            Directory.CreateDirectory(TEST_DIR);

            var file = File.Create($@"{TEST_DIR}\test-file.png");
            file.Close();
        }


        [TearDown]
        public void TearDown()
        {
            DeleteFolderWithContents(_createdTestDir);
            DeleteFolderWithContents(TEST_DIR);

            _createdTestDir = string.Empty;
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Deletes the given <paramref name="folder"/> with its contents.
        /// </summary>
        /// <param name="folder">The folder to delete.</param>
        private void DeleteFolderWithContents(string folder)
        {
            if (Directory.Exists(folder))
            {
                var filesToDelete = Directory.GetFiles(folder);

                foreach (var file in filesToDelete)
                {
                    File.Delete(file);
                }

                Directory.Delete(folder);
            }
        }
        #endregion
    }
}
