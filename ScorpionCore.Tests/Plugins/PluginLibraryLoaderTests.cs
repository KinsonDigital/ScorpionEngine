using NUnit.Framework;
using ScorpionCore.Exceptions;
using ScorpionCore.Plugins;

namespace ScorpionCore.Tests.Plugins
{
    [TestFixture]
    public class PluginLibraryLoaderTests
    {
        [Test]
        public void LoadPluginLibrary_WhenInvokingWithInvalidName_ThrowsException()
        {
            //Arrange
            var pluginName = "MyPlugin";

            //Act/Assert
            Assert.Throws<PluginNotFoundException>(() => PluginLibraryLoader.LoadPluginLibrary(pluginName));
        }
    }
}
