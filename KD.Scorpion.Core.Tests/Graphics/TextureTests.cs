using Moq;
using NUnit.Framework;
using KDScorpionCore.Graphics;
using KDScorpionCoreTests.Fakes;

namespace KDScorpionCoreTests.Graphics
{
    public class TextureTests
    {
        [Test]
        public void GetTexture_WhenInvoking_ReturnsTexture()
        {
            //Arrange
            var mockTexture = new Mock<ITexture>();
            mockTexture.Setup(m => m.GetTextureAsClass<FakeTexture>()).Returns(new FakeTexture() { FakeData = 11 });
            var texture = new Texture(mockTexture.Object);

            var expected = new FakeTexture() { FakeData = 11 };

            //Act
            var actual = texture.GetTexture<FakeTexture>();

            //Assert
            Assert.AreEqual(expected.FakeData, actual.FakeData);
        }


        [Test]
        public void Width_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockTexture = new Mock<ITexture>();
            mockTexture.SetupGet(m => m.Width).Returns(23);
            var texture = new Texture(mockTexture.Object);
            var expected = 23;

            //Act
            var actual = texture.Width;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Height_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockTexture = new Mock<ITexture>();
            mockTexture.SetupGet(m => m.Height).Returns(56);
            var texture = new Texture(mockTexture.Object);

            var expected = 56;

            //Act
            var actual = texture.Height;

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
