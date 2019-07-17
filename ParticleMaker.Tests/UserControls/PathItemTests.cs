using Xunit;
using ParticleMaker.UserControls;

namespace ParticleMaker.Tests.UserControls
{
    /// <summary>
    /// Unit tests to test the <see cref="PathItem"/> class.
    /// </summary>
    public class PathItemTests
    {
        #region Prop Tests
        [Fact]
        public void FilePath_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var item = new PathItem();
            var expected = "test-item";

            //Act
            item.FilePath = "test-item";
            var actual = item.FilePath;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Method Tests
        [Fact]
        public void Equals_WhenInvokingWithEqualItem_ReturnsTrue()
        {
            //Arrange
            var itemA = new PathItem() { FilePath = "test-item" };
            var itemB = new PathItem() { FilePath = "test-item" };

            var expected = true;

            //Act
            var actual = itemA.Equals(itemB);

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Equals_WhenInvokingWithIncorrectObjectType_ReturnsFalse()
        {
            //Arrange
            var itemA = new PathItem() { FilePath = "test-item-A" };
            var itemB = new object();

            var expected = false;

            //Act
            var actual = itemA.Equals(itemB);

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void GetHashCode_WhenInvoking_ReturnsCorrectValue()
        {
            //Arrange
            var item = new PathItem() { FilePath = "test-item-A" };

            //Assert
            Assert.NotEqual(0, item.GetHashCode());
        }
        #endregion
    }
}
