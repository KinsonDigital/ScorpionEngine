using Xunit;

namespace PluginSystem.Tests
{
    /// <summary>
    /// Unit tests to test the <see cref="LibChooser"/> class.
    /// </summary>
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
