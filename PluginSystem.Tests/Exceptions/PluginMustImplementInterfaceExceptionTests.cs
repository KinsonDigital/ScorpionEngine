using Xunit;

namespace PluginSystem.Exceptions
{
    public class PluginMustImplementInterfaceExceptionTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvokingWithNoParam_CorrectlySetsExceptionMessage()
        {
            //Arrange
            var expected = $"The plugin must implement the IPlugin interface.";
            var exception = new PluginMustImplementInterfaceException();

            //Act
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Ctor_WhenInvokingWithNameParam_CorrectlySetsExceptionMessage()
        {
            //Arrange
            var expected = $"The plugin TestPlugin must implement the IPlugin interface.";
            var exception = new PluginMustImplementInterfaceException("TestPlugin");

            //Act
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}
