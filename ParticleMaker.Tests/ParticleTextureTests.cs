using Moq;
using System;
using System.Drawing;
using Xunit;

namespace ParticleMaker.Tests
{
    public class ParticleTextureTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_CorrectlySetsProps()
        {
            //Arrange
            var texture = new ParticleTexture(new IntPtr(1234), 11, 22);

            //Act & Assert
            Assert.Equal(new IntPtr(1234), texture.TexturePointer);
            Assert.Equal(11, texture.Width);
            Assert.Equal(22, texture.Height);
        }
        #endregion


        #region Prop Tests
        [Fact]
        public void Name_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange & Act
            var texture = new ParticleTexture(It.IsAny<IntPtr>(), It.IsAny<int>(), It.IsAny<int>())
            {
                Name = "TestName"
            };

            //Assert
            Assert.Equal("TestName", texture.Name);
        }


        [Fact]
        public void TexturePointer_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange & Act
            var texture = new ParticleTexture(It.IsAny<IntPtr>(), It.IsAny<int>(), It.IsAny<int>())
            {
                TexturePointer = new IntPtr(1234)
            };

            //Assert
            Assert.Equal(new IntPtr(1234), texture.TexturePointer);
        }


        [Fact]
        public void X_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange & Act
            var texture = new ParticleTexture(It.IsAny<IntPtr>(), It.IsAny<int>(), It.IsAny<int>())
            {
                X = 1234
            };

            //Assert
            Assert.Equal(1234, texture.X);
        }


        [Fact]
        public void Y_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange & Act
            var texture = new ParticleTexture(It.IsAny<IntPtr>(), It.IsAny<int>(), It.IsAny<int>())
            {
                Y = 1234
            };

            //Assert
            Assert.Equal(1234, texture.Y);
        }


        [Fact]
        public void Color_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange & Act
            var texture = new ParticleTexture(It.IsAny<IntPtr>(), It.IsAny<int>(), It.IsAny<int>())
            {
                Color = Color.FromArgb(11, 22, 33, 44)
            };

            //Assert
            Assert.Equal(Color.FromArgb(11, 22, 33, 44), texture.Color);
        }
        #endregion


        #region Method Tests
        [Fact]
        public void GetHashCode_WhenInvoking_ReturnsCorrectValue()
        {
            //Arrange & Act
            var texture = new ParticleTexture(It.IsAny<IntPtr>(), It.IsAny<int>(), It.IsAny<int>())
            {
                Name = "TestName"
            };

            //Act & Assert
            Assert.NotEqual(0, texture.GetHashCode());
        }
        #endregion
    }
}
