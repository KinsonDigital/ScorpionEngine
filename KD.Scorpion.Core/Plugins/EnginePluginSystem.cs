using System;

namespace KDScorpionCore.Plugins
{
    public static class EnginePluginSystem
    {
        private static PluginSystem.Plugins _plugins;


        public static PluginSystem.Plugins Plugins
        {
            get
            {
                if (_plugins == null)
                    throw new Exception($"The plugin system has not been set.  Please invoke the '{nameof(SetPlugins)}'() method to set the plugin system.");


                return _plugins;
            }
        }


        public static void SetPlugins(PluginSystem.Plugins plugins) => _plugins = plugins;


        public static void ClearPlugins() => _plugins = null;
    }
}
