using Xunit;
using ParticleMaker.CustomEventArgs;

namespace ParticleMaker.Tests.CustomEventArgs
{
    public class RenameItemEventArgsTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_ProperlySetsPropValues()
        {
            //Arrange
            var expectedOldName = "old-name";
            var expectedOldPath = "old-path";

            //Act
            var eventArgs = new RenameItemEventArgs("old-name", "old-path");
            var actualOldName = eventArgs.OldName;
            var actualOldPath = eventArgs.OldPath;

            //Assert
            Assert.Equal(expectedOldName, actualOldName);
            Assert.Equal(expectedOldPath, actualOldPath);
        }
        #endregion


        #region Prop Tests
        [Fact]
        public void OldName_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var eventArgs = new RenameItemEventArgs("", "");
            var expected = "old-name";

            //Act
            eventArgs.OldName = "old-name";
            var actual = eventArgs.OldName;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void NewName_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var eventArgs = new RenameItemEventArgs("", "");
            var expected = "new-name";

            //Act
            eventArgs.NewName = "new-name";
            var actual = eventArgs.NewName;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void OldPath_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var eventArgs = new RenameItemEventArgs("", "");
            var expected = "old-path";

            //Act
            eventArgs.OldPath = "old-path";
            var actual = eventArgs.OldPath;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void NewPath_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var eventArgs = new RenameItemEventArgs("", "");
            var expected = "new-path";

            //Act
            eventArgs.NewPath = "new-path";
            var actual = eventArgs.NewPath;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}
