using System;
using System.Collections.Generic;
using System.Text;
using KDScorpionEngine.Content;
using Moq;
using Raptor.Graphics;
using Xunit;

namespace KDScorpionEngineTests.Content
{
    public class AtlasRepositoryTests : IDisposable
    {
        private readonly AtlasRepository atlasRepo;

        public AtlasRepositoryTests()
        {
            this.atlasRepo = AtlasRepository.Instance;
        }

        [Fact]
        public void AddAtlasData_WithAlreadyExistingAtlasData_ThrowsException()
        {
            // Arrange
            var atlasSpriteData = new List<AtlasSpriteData>();
            var atlasData = new AtlasData(atlasSpriteData.ToArray(), new Mock<ITexture>().Object, "test-name");
            this.atlasRepo.AddAtlasData("atlasA", atlasData);

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                this.atlasRepo.AddAtlasData("atlasA", atlasData);
            });
        }

        [Fact]
        public void RemoveAtlasData_WhenAtlasDataDoesNotExist_ThrowsException()
        {
            // Arrange
            var atlasSpriteData = new List<AtlasSpriteData>();
            var atlasData = new AtlasData(atlasSpriteData.ToArray(), new Mock<ITexture>().Object, "test-name");

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                this.atlasRepo.RemoveAtlasData("atlasA");
            });
        }

        [Fact]
        public void RemoveAtlasData_WhenAtlasDoesExist_RemovesAtlasData()
        {
            // Arrange
            var atlasSpriteData = new List<AtlasSpriteData>();
            var atlasData = new AtlasData(atlasSpriteData.ToArray(), new Mock<ITexture>().Object, "test-name");
            this.atlasRepo.AddAtlasData("atlasA", atlasData);
            var countAfterAdding = this.atlasRepo.TotalItems;

            // Act
            this.atlasRepo.RemoveAtlasData("atlasA");
            var countAfterRemoving = this.atlasRepo.TotalItems;

            // Assert
            Assert.Equal(1, countAfterAdding);
            Assert.Equal(0, countAfterRemoving);
        }

        [Fact]
        public void EmptyRepository_WhenInvoked_EmptiesRepository()
        {
            // Arrange
            var atlasSpriteData = new List<AtlasSpriteData>();
            var atlasData = new AtlasData(atlasSpriteData.ToArray(), new Mock<ITexture>().Object, "test-name");

            this.atlasRepo.AddAtlasData("atlasA", atlasData);

            // Act
            this.atlasRepo.EmptyRepository();
            var actual = this.atlasRepo.TotalItems;

            // Assert
            Assert.Equal(0, actual);
        }

        public void Dispose()
        {
            this.atlasRepo.EmptyRepository();
        }
    }
}
