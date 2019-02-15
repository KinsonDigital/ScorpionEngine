using NUnit.Framework;
using ParticleMaker.UserControls;

namespace ParticleMaker.Tests.UserControls
{
    [TestFixture]
    public class PathItemTests
    {
        #region Prop Tests
        [Test]
        public void FilePath_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var item = new PathItem();
            var expected = "test-item";

            //Act
            item.FilePath = "test-item";
            var actual = item.FilePath;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Method Tests
        [Test]
        public void Equals_WhenInvokingWithEqualItem_ReturnsTrue()
        {
            //Arrange
            var itemA = new PathItem() { FilePath = "test-item" };
            var itemB = new PathItem() { FilePath = "test-item" };

            var expected = true;

            //Act
            var actual = itemA.Equals(itemB);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Equals_WhenInvokingWithIncorrectObjectType_ReturnsFalse()
        {
            //Arrange
            var itemA = new PathItem() { FilePath = "test-item-A" };
            var itemB = new object();

            var expected = false;

            //Act
            var actual = itemA.Equals(itemB);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void GetHashCode_WhenInvoking_ReturnsCorrectValue()
        {
            //Arrange
            var itemA = new PathItem() { FilePath = "test-item-A" };

            var expected = 1930328289;

            //Act
            var actual = itemA.GetHashCode();

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
