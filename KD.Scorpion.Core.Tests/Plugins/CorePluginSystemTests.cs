using KDScorpionCore.Plugins;
using System;
using Xunit;

namespace KDScorpionCoreTests.Plugins
{
    /// <summary>
    /// Unit tests to test the <see cref="CorePluginSystem"/> class.
    /// </summary>
    public class CorePluginSystemTests : IDisposable
    {
        #region Prop Tests
        [Fact]
        public void Plugins_WhenGettingValueWithNullValue_ThrowsException()
        {
            //Assert
            Assert.Throws<Exception>(() => CorePluginSystem.Plugins);
        }
        #endregion


        #region Method Tests
        [Fact]
        public void SetPlugins_WhenInvoking_SetsPlugins()
        {
            //Act
            CorePluginSystem.SetPlugins(new PluginSystem.Plugins());

            //Assert
            Assert.NotNull(CorePluginSystem.Plugins);
        }
        #endregion


        #region Public Methods
        public void Dispose() => CorePluginSystem.ClearPlugins();
        #endregion
    }
}
