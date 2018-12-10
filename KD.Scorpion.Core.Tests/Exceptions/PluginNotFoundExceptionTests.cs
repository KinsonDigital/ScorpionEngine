using NUnit.Framework;
using KDScorpionCore.Exceptions;

namespace KDScorpionCoreTests.Exceptions
{
    [TestFixture]
    public class PluginNotFoundExceptionTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvokingWithNoParam_CorrectlySetsExceptionMessage()
        {
            //Arrange
            var expected = "The plugin could not be found in the plugin assembly";
            var exception = new PluginNotFoundException();

            //Act
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvokingWithTwoParams_CorrectlySetsExceptionMessage()
        {
            //Arrange
            var pluginName = "MyPlugin";
            var pluginAssembly = "TestPlugin";
            var expected = $"The plugin '{pluginName}' could not be found in the plugin assembly '{pluginAssembly}.dll'.";
            var exception = new PluginNotFoundException(pluginName, pluginAssembly);

            //Act
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
