using NUnit.Framework;
using ParticleMaker.CustomEventArgs;

namespace ParticleMaker.Tests.CustomEventArgs
{
    [TestFixture]
    public class DeleteItemEventArgsTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvoking_ProperlySetsPropValues()
        {
            //Arrange
            var expectedName = "name";
            var expectedPath = "path";

            //Act
            var eventArgs = new DeleteItemEventArgs("name", "path");
            var actualName = eventArgs.Name;
            var actualPath = eventArgs.Path;

            //Assert
            Assert.AreEqual(expectedName, actualName);
            Assert.AreEqual(expectedPath, actualPath);
        }
        #endregion


        #region Prop Tests
        [Test]
        public void Name_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var eventArgs = new DeleteItemEventArgs(string.Empty, string.Empty);
            var expected = "name";

            //Act
            eventArgs.Name = "name";
            var actual = eventArgs.Name;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Path_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var eventArgs = new DeleteItemEventArgs(string.Empty, string.Empty);
            var expected = "path";

            //Act
            eventArgs.Path = "path";
            var actual = eventArgs.Path;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
