using PluginSystem;
using System;

namespace KDScorpionEngine
{
    public static class EnginePluginSystem
    {
        private static Plugins _plugins;


        public static Plugins Plugins
        {
            get
            {
                if (_plugins == null)
                    throw new Exception($"The plugin system has not been set.  Please invoke the '{nameof(SetPlugin)}'() method to set the plugin system.");


                return _plugins;
            }
        }


        public static void SetPlugin(Plugins plugins) => _plugins = plugins;


        public static void ClearPlugin() => _plugins = null;
    }
}
