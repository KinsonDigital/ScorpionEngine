using NUnit.Framework;
using ParticleMaker.CustomEventArgs;

namespace ParticleMaker.Tests.CustomEventArgs
{
    [TestFixture]
    public class RenameSetupItemEventArgsTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvoking_ProperlySetsPropValues()
        {
            //Arrange
            var expectedOldName = "old-name";
            var expectedOldPath = "old-path";

            //Act
            var eventArgs = new RenameSetupItemEventArgs("old-name", "old-path");
            var actualOldName = eventArgs.OldName;
            var actualOldPath = eventArgs.OldPath;

            //Assert
            Assert.AreEqual(expectedOldName, actualOldName);
            Assert.AreEqual(expectedOldPath, actualOldPath);
        }
        #endregion


        #region Prop Tests
        [Test]
        public void OldName_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var eventArgs = new RenameSetupItemEventArgs("", "");
            var expected = "old-name";

            //Act
            eventArgs.OldName = "old-name";
            var actual = eventArgs.OldName;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void NewName_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var eventArgs = new RenameSetupItemEventArgs("", "");
            var expected = "new-name";

            //Act
            eventArgs.NewName = "new-name";
            var actual = eventArgs.NewName;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void OldPath_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var eventArgs = new RenameSetupItemEventArgs("", "");
            var expected = "old-path";

            //Act
            eventArgs.OldPath = "old-path";
            var actual = eventArgs.OldPath;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void NewPath_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var eventArgs = new RenameSetupItemEventArgs("", "");
            var expected = "new-path";

            //Act
            eventArgs.NewPath = "new-path";
            var actual = eventArgs.NewPath;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
