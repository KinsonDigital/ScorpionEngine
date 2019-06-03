﻿using System.Diagnostics.CodeAnalysis;

namespace KDScorpionCore.Plugins
{
    [ExcludeFromCodeCoverage]
    internal static class PluginSystemOLD
    {
        #region Props
        public static IPluginLibrary EnginePlugins { get; private set; }

        public static IPluginLibrary PhysicsPlugins { get; private set; }
        #endregion


        #region Public Methods
        public static void LoadEnginePluginLibrary(IPluginLibrary library)
        {
            EnginePlugins = library;
        }


        public static void LoadPhysicsPluginLibrary(IPluginLibrary library)
        {
            PhysicsPlugins = library;
        }


        public static void ClearPlugins()
        {
            EnginePlugins = null;
            PhysicsPlugins = null;
        }
        #endregion
    }
}
