using NUnit.Framework;
using ScorpionCore.Exceptions;
using ScorpionCore.Plugins;

namespace ScorpionCore.Tests.Exceptions
{
    [TestFixture]
    public class PluginMustImplementInterfaceExceptionTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvokingWithNoParam_CorrectlySetsExceptionMessage()
        {
            //Arrange
            var expected = $"The plugin must implement the {nameof(IPlugin)} interface.";
            var exception = new PluginMustImplementInterfaceException();

            //Act
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvokingWithNameParam_CorrectlySetsExceptionMessage()
        {
            //Arrange
            var expected = $"The plugin TestPlugin must implement the {nameof(IPlugin)} interface.";
            var exception = new PluginMustImplementInterfaceException("TestPlugin");

            //Act
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
