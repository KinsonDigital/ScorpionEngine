using Xunit;

namespace PluginSystem.Exceptions
{
    /// <summary>
    /// Unit tests to test the <see cref="PluginNotFoundException"/> class.
    /// </summary>
    public class PluginNotFoundExceptionTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvokingWithNoParam_CorrectlySetsExceptionMessage()
        {
            //Arrange
            var expected = "The plugin could not be found in the plugin assembly";
            var exception = new PluginNotFoundException();

            //Act
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
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
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}
