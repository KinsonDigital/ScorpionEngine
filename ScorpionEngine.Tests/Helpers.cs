using Moq;
using ScorpionCore;
using ScorpionCore.Plugins;
using ScorpionEngine.Physics;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ScorpionEngine.Tests
{
    [ExcludeFromCodeCoverage]
    public static class Helpers
    {
        /// <summary>
        /// Sets up the plugin system to pull in the required object by the given generic type of <typeparamref name="PluginMock"/>.
        /// </summary>
        /// <typeparam name="PluginMock">The type of mocked plugin to load into the plugin system.  <typeparamref name="PluginMock"/> must implement an interface of type <see cref="IPlugin"/>.</typeparam>
        public static void SetupPluginLib<PluginMock>(PluginLibType libType) where PluginMock : class, IPlugin
        {
            var mock = new Mock<PluginMock>();
            var mockPluginLib = new Mock<IPluginLibrary>();
            mockPluginLib.Setup(m => m.LoadPlugin<PluginMock>()).Returns(mock.Object);

            LoadPluginLibrary(mockPluginLib, libType);
        }


        public static void SetupPluginLib<PluginMock>(Mock<PluginMock> mockObject, PluginLibType libType) where PluginMock : class, IPlugin
        {
            var mockPluginLib = new Mock<IPluginLibrary>();
            mockPluginLib.Setup(m => m.LoadPlugin<PluginMock>()).Returns(() =>
            {
                return mockObject.Object;
            });


            LoadPluginLibrary(mockPluginLib, libType);
        }


        public static void SetupPluginLib<PluginMock, Param1>(Mock<PluginMock> pluginMock, PluginLibType libType) where PluginMock : class, IPlugin
        {
            var mockPluginLib = new Mock<IPluginLibrary>();
            mockPluginLib.Setup(m => m.LoadPlugin<PluginMock>(It.IsAny<Param1>())).Returns(() =>
            {
                return pluginMock.Object;
            });


            LoadPluginLibrary(mockPluginLib, libType);
        }


        public static void SetupPluginLib<PluginMock, Param1>(Mock<PluginMock> pluginMock, PluginLibType libType, Param1 param) where PluginMock : class, IPlugin
        {
            var mockPluginLib = new Mock<IPluginLibrary>();
            mockPluginLib.Setup(m => m.LoadPlugin<PluginMock>(param)).Returns(() =>
            {
                return pluginMock.Object;
            });


            LoadPluginLibrary(mockPluginLib, libType);
        }


        public static void SetupPluginLib<PluginMock, Param1, Param2>(Mock<PluginMock> pluginMock, PluginLibType libType) where PluginMock : class, IPlugin
        {
            var mockPluginLib = new Mock<IPluginLibrary>();
            mockPluginLib.Setup(m => m.LoadPlugin<PluginMock>(It.IsAny<Param1>(), It.IsAny<Param2>())).Returns(() =>
            {
                return pluginMock.Object;
            });


            LoadPluginLibrary(mockPluginLib, libType);
        }


        private static void LoadPluginLibrary<PluginLib>(Mock<PluginLib> pluginLibMock, PluginLibType libType) where PluginLib : class, IPluginLibrary
        {
            switch (libType)
            {
                case PluginLibType.Engine:
                    PluginSystem.LoadEnginePluginLibrary(pluginLibMock.Object);
                    break;
                case PluginLibType.Physics:
                    PluginSystem.LoadPhysicsPluginLibrary(pluginLibMock.Object);
                    break;
                default:
                    throw new Exception($"Unknown plugin library type of {libType}");
            }
        }
    }
}
