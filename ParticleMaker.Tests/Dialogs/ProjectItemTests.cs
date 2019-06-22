using Xunit;
using ParticleMaker.Dialogs;

namespace ParticleMaker.Tests.Dialogs
{
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
    }
}
