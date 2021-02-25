using System.Drawing;
using System.Numerics;
using KDScorpionEngine.Graphics;
using Moq;
using Raptor.Content;
using Xunit;

namespace KDScorpionEngineTests.Graphics
{
    /// <summary>
    /// Tests the <see cref="RenderSection"/> class.
    /// </summary>
    public class RenderSectionTests
    {
        #region Prop Tests
        [Fact]
        public void TextureName_WhenSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var section = new RenderSection();

            // Act
            section.TextureName = "texture";

            // Assert
            Assert.Equal("texture", section.TextureName);
        }

        [Fact]
        public void SubTextureName_WhenSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var section = new RenderSection();

            // Act
            section.SubTextureName = "sub-texture";

            // Assert
            Assert.Equal("sub-texture", section.SubTextureName);
        }

        [Fact]
        public void RenderBounds_WhenSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var section = new RenderSection();

            // Act
            section.RenderBounds = new Rectangle(10, 20, 30, 40);

            // Assert
            Assert.Equal(new Rectangle(10, 20, 30, 40), section.RenderBounds);
        }

        [Fact]
        public void HalfWidth_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var section = new RenderSection();
            section.RenderBounds = new Rectangle(10, 20, 30, 40);

            // Act
            var actual = section.HalfWidth;

            // Assert
            Assert.Equal(15, actual);
        }

        [Fact]
        public void HalfHeight_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var section = new RenderSection();
            section.RenderBounds = new Rectangle(10, 20, 30, 40);

            // Act
            var actual = section.HalfHeight;

            // Assert
            Assert.Equal(20, actual);
        }

        [Fact]
        public void TypeOfTexture_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var section = new RenderSection();

            // Act
            section.TypeOfTexture = TextureType.SubTexture;

            // Assert
            Assert.Equal(TextureType.SubTexture, section.TypeOfTexture);
        }
        #endregion

        #region Method Tests
        [Fact]
        public void Reset_WhenInvoked_ResetsSection()
        {
            // Arrange
            var section = new RenderSection();
            section.TextureName = nameof(RenderSection.TextureName);
            section.SubTextureName = nameof(RenderSection.SubTextureName);
            section.RenderBounds = new Rectangle(11, 22, 33, 44);
            section.TypeOfTexture = TextureType.SubTexture;

            // Act
            section.Reset();

            // Assert
            Assert.Equal(string.Empty, section.TextureName);
            Assert.Equal(string.Empty, section.SubTextureName);
            Assert.Equal(Rectangle.Empty, section.RenderBounds);
            Assert.Equal(TextureType.WholeTexture, section.TypeOfTexture);
        }

        [Fact]
        public void GetTexturePosition_WhenAnimated_ReturnsCorrectTexturePosition()
        {
            // Arrange
            var section = new RenderSection();
            section.RenderBounds = new Rectangle(11, 22, 33, 44);
            section.Animator = new Mock<IAnimator>().Object;

            // Act
            var actual = section.GetTexturePosition();

            // Assert
            Assert.Equal(new Vector2(11, 22), actual);
        }

        [Fact]
        public void GetTexturePosition_WhenNotAnimatedWithWholeTexture_ReturnsCorrectTexturePosition()
        {
            // Arrange
            var section = new RenderSection();
            section.RenderBounds = new Rectangle(11, 22, 33, 44);
            section.TypeOfTexture = TextureType.WholeTexture;

            // Act
            var actual = section.GetTexturePosition();

            // Assert
            Assert.Equal(Vector2.Zero, actual);
        }

        [Fact]
        public void GetTexturePosition_WhenNotAnimatedWithSubTexture_ReturnsCorrectTexturePosition()
        {
            // Arrange
            var section = new RenderSection();
            section.RenderBounds = new Rectangle(11, 22, 33, 44);
            section.TypeOfTexture = TextureType.SubTexture;

            // Act
            var actual = section.GetTexturePosition();

            // Assert
            Assert.Equal(new Vector2(11, 22), actual);
        }

        [Fact]
        public void GetTexturePosition_WhenNotAnimatedWithUnknownTextureType_ReturnsCorrectTexturePosition()
        {
            // Arrange
            var section = new RenderSection();
            section.RenderBounds = new Rectangle(11, 22, 33, 44);
            section.TypeOfTexture = (TextureType)44;

            // Act
            var actual = section.GetTexturePosition();

            // Assert
            Assert.Equal(Vector2.Zero, actual);
        }

        [Fact]
        public void CreateNonAnimatedWholeTexture_WhenInvoked_ReturnsCorrectSection()
        {
            // Act
            var section = RenderSection.CreateNonAnimatedWholeTexture("whole-texture");

            // Assert
            Assert.Equal("whole-texture", section.TextureName);
            Assert.Equal(string.Empty, section.SubTextureName);
            Assert.Equal(TextureType.WholeTexture, section.TypeOfTexture);
        }

        [Fact]
        public void CreateNonAnimatedSubTexture_WhenInvoked_ReturnsCorrectSection()
        {
            // Act
            var section = RenderSection.CreateNonAnimatedSubTexture("texture-atlas", "sub-texture");

            // Assert
            Assert.Equal("texture-atlas", section.TextureName);
            Assert.Equal("sub-texture", section.SubTextureName);
            Assert.Equal(TextureType.SubTexture, section.TypeOfTexture);
        }

        [Fact]
        public void CreateAnimatedSubTexture_WhenInvoked_ReturnsCorrectSection()
        {
            // Act
            var section = RenderSection.CreateAnimatedSubTexture("texture-atlas", "sub-texture");

            // Assert
            Assert.Equal("texture-atlas", section.TextureName);
            Assert.Equal("sub-texture", section.SubTextureName);
            Assert.Equal(TextureType.SubTexture, section.TypeOfTexture);
        }

        [Fact]
        public void CreateAnimatedSubTexture_WithAnimator_ReturnsCorrectSection()
        {
            // Act
            var mockAnimator = new Mock<IAnimator>();
            var section = RenderSection.CreateAnimatedSubTexture("texture-atlas", "sub-texture", mockAnimator.Object);

            // Assert
            Assert.Equal("texture-atlas", section.TextureName);
            Assert.Equal("sub-texture", section.SubTextureName);
            Assert.Same(mockAnimator.Object, section.Animator);
            Assert.Equal(TextureType.SubTexture, section.TypeOfTexture);
        }
        #endregion
    }
}
