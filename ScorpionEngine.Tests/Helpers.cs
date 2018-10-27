using Moq;
using ScorpionCore;
using ScorpionCore.Plugins;
using System;

namespace ScorpionEngine.Tests
{
    public static class Helpers
    {
        /// <summary>
        /// Sets up the plugin system to pull in the required object by the given generic type of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of mocked plugin to load into the plugin system.  <typeparamref name="T"/> must implement an interface of type <see cref="IPlugin"/>.</typeparam>
        public static void SetupEnginePluginLib<T>(PluginLibType libType) where T : class, IPlugin
        {
            var mockMouse = new Mock<T>();
            var mockPluginLib = new Mock<IPluginLibrary>();
            mockPluginLib.Setup(m => m.LoadPlugin<T>()).Returns(mockMouse.Object);

            switch (libType)
            {
                case PluginLibType.Engine:
                    PluginSystem.LoadEnginePluginLibrary(mockPluginLib.Object);
                    break;
                case PluginLibType.Physics:
                    PluginSystem.LoadPhysicsPluginLibrary(mockPluginLib.Object);
                    break;
                default:
                    throw new Exception($"Unknown plugin library type of {libType}");
            }
        }


        public static void SetupEnginePluginLib<T>(Mock<T> mockObject, PluginLibType libType) where T : class, IPlugin
        {
            var mockPluginLib = new Mock<IPluginLibrary>();
            mockPluginLib.Setup(m => m.LoadPlugin<T>()).Returns(mockObject.Object);

            switch (libType)
            {
                case PluginLibType.Engine:
                    PluginSystem.LoadEnginePluginLibrary(mockPluginLib.Object);
                    break;
                case PluginLibType.Physics:
                    PluginSystem.LoadPhysicsPluginLibrary(mockPluginLib.Object);
                    break;
                default:
                    throw new Exception($"Unknown plugin library type of {libType}");
            }
        }
    }
}
