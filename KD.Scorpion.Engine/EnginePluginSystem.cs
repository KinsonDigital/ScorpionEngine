using PluginSystem;
using System;

namespace KDScorpionEngine
{
    public static class EnginePluginSystem
    {
        private static Plugins_NEW _plugins;


        public static Plugins_NEW Plugins
        {
            get
            {
                if (_plugins == null)
                    throw new Exception($"The plugin system has not been set.  Please invoke the '{nameof(SetPlugin)}'() method to set the plugin system.");


                return _plugins;
            }
        }


        public static void SetPlugin(Plugins_NEW plugins) => _plugins = plugins;


        public static void ClearPlugin() => _plugins = null;
    }
}
