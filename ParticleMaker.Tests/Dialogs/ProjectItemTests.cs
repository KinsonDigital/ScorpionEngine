using Xunit;
using ParticleMaker.Dialogs;

namespace ParticleMaker.Tests.Dialogs
{
    /// <summary>
    /// Unit tests to test the <see cref="ProjectItem"/> class.
    /// </summary>
    public class ProjectItemTests
    {
        #region Prop Tests
        [Fact]
        public void Name_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var item = new ProjectItem();
            var expected = "test-name";

            //Act
            item.Name = "test-name";
            var actual = item.Name;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Exists_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var item = new ProjectItem();
            var expected = true;

            //Act
            item.Exists = true;
            var actual = item.Exists;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Method Tests
        [Fact]
        public void Equals_WhenInvokedWithEqualItem_ReturnsTrue()
        {
            //Arrange
            var itemA = new ProjectItem()
            {
                Exists = true,
                Name = "item"
            };
            var itemB = new ProjectItem()
            {
                Exists = true,
                Name = "item"
            };

            //Act & Assert
            Assert.True(itemA.Equals(itemB));
        }


        [Fact]
        public void Equals_WhenInvokedWithUnequalItem_ReturnsFalse()
        {
            //Arrange
            var itemA = new ProjectItem()
            {
                Exists = true,
                Name = "itemA"
            };
            var itemB = new ProjectItem()
            {
                Exists = true,
                Name = "itemB"
            };

            //Act & Assert
            Assert.False(itemA.Equals(itemB));
        }


        [Fact]
        public void GetHashCode_WhenInvoked_ReturnsCorrectValue()
        {
            //Arrange
            var item = new ProjectItem()
            {
                Exists = true,
                Name = "item"
            };

            //Act & Assert
            Assert.NotEqual(0, item.GetHashCode());
        }
        #endregion
    }
}
