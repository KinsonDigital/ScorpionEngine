using Moq;
using ScorpionCore;
using ScorpionEngine.Graphics;
using ScorpionEngine.Tests.Fakes;
using Xunit;

namespace ScorpionEngine.Tests
{
    public class TextureTests
    {
        [Fact]
        public void GetTexture_WhenInvoking_ReturnsTexture()
        {
            //Arrange
            var mockTexture = new Mock<ITexture>();
            mockTexture.Setup(m => m.GetTexture<FakeTexture>()).Returns(new FakeTexture() { FakeData = 11 });
            var texture = new Texture()
            {
                InternalTexture = mockTexture.Object
            };
            var expected = new FakeTexture() { FakeData = 11 };

            //Act
            var actual = texture.GetTexture<FakeTexture>();

            //Assert
            Assert.Equal(expected.FakeData, actual.FakeData);
        }


        [Fact]
        public void Width_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockTexture = new Mock<ITexture>();
            mockTexture.SetupGet(m => m.Width).Returns(23);
            var texture = new Texture()
            {
                InternalTexture = mockTexture.Object
            };
            var expected = 23;

            //Act
            var actual = texture.Width;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Width_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockTexture = new Mock<ITexture>();
            mockTexture.SetupProperty(m => m.Width);
            var texture = new Texture()
            {
                InternalTexture = mockTexture.Object
            };
            var expected = 44;

            //Act
            texture.Width = 44;
            var actual = texture.Width;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Height_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockTexture = new Mock<ITexture>();
            mockTexture.SetupGet(m => m.Height).Returns(56);
            var texture = new Texture()
            {
                InternalTexture = mockTexture.Object
            };
            var expected = 56;

            //Act
            var actual = texture.Height;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Height_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockTexture = new Mock<ITexture>();
            mockTexture.SetupProperty(m => m.Height);
            var texture = new Texture()
            {
                InternalTexture = mockTexture.Object
            };
            var expected = 9;

            //Act
            texture.Height = 9;
            var actual = texture.Height;

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
