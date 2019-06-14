using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(assemblyName: "ScorpionEngineTests", AllInternalsVisible = true)]

namespace PluginSystem
{
    [ExcludeFromCodeCoverage]
    public static class Plugins
    {
        #region Private Fields
        private static IPluginLibrary _enginePluginLib;
        private static IPluginLibrary _physicsPluginLib;
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the name of the engine plugin library.
        /// </summary>
        public static string EngineLibraryPluginName { get; set; } = LibChooser.EnginePluginLibraryName;

        /// <summary>
        /// Gets or sets the name of the engine plugin library.
        /// </summary>
        public static string PhysicsLibraryPluginName { get; set; } = LibChooser.PhysicsPLuginLibraryName;
        
        /// <summary>
        /// Gets the loaded engine plugin library.
        /// </summary>
        public static IPluginLibrary EnginePlugins
        {
            get
            {
                if (string.IsNullOrEmpty(EngineLibraryPluginName))
                    throw new Exception($"The {nameof(EngineLibraryPluginName)} property must be set to the name of the plugin properly load the engine plugin library.");

                if (_enginePluginLib is null)
                    _enginePluginLib = new PluginLibrary(PluginLibraryLoader.LoadPluginLibrary(EngineLibraryPluginName));


                return _enginePluginLib;
            }
            internal set => _enginePluginLib = value;
        }

        /// <summary>
        /// Gets the loaded physics plugin library.
        /// </summary>
        public static IPluginLibrary PhysicsPlugins
        {
            get
            {
                if (string.IsNullOrEmpty(PhysicsLibraryPluginName))
                    throw new Exception($"The {nameof(PhysicsLibraryPluginName)} property must be set to the name of the plugin to load to properly load the physics plugin library.");

                if (_physicsPluginLib is null)
                    _physicsPluginLib = new PluginLibrary(PluginLibraryLoader.LoadPluginLibrary(PhysicsLibraryPluginName));


                return _physicsPluginLib;
            }
            internal set => _physicsPluginLib = value;
        }
        #endregion
    }
}
