using System;
using System.Diagnostics.CodeAnalysis;

namespace PluginSystem
{
    /// <summary>
    /// Holds the plugin implementations from a loaded plugin library.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Plugins
    {
        #region Private Fields
        private IPluginLibrary _enginePluginLib;
        private IPluginLibrary _physicsPluginLib;
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the name of the game engine plugin library.
        /// </summary>
        public string EngineLibraryPluginName { get; set; } = LibChooser.EnginePluginLibraryName;

        /// <summary>
        /// Gets or sets the name of the physics engine plugin library.
        /// </summary>
        public string PhysicsLibraryPluginName { get; set; } = LibChooser.PhysicsPLuginLibraryName;

        /// <summary>
        /// Gets the loaded game engine plugin library.
        /// </summary>
        public IPluginLibrary EnginePlugins
        {
            [ExcludeFromCodeCoverage]
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
        /// Gets the loaded physics engine plugin library.
        /// </summary>
        public IPluginLibrary PhysicsPlugins
        {
            [ExcludeFromCodeCoverage]
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
