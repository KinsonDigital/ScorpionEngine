using Xunit;

namespace PluginSystem.Tests
{
    public class LibChooserTests
    {
        [Fact]
        public void PhysicsPluginLibraryName_WhenGettingValue_ReturnsCorrectResult()
        {
            //Act & Assert
            Assert.Equal("VelcroPhysicsPlugin", LibChooser.PhysicsPLuginLibraryName);
        }
    }
}
