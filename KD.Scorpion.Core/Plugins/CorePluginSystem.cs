using System;

namespace KDScorpionCore.Plugins
{
    /// <summary>
    /// Provides a wrapper around a loaded plugin library for easy access.
    /// </summary>
    public static class CorePluginSystem
    {
        #region Private Fields
        private static PluginSystem.Plugins _plugins;
        #endregion


        #region Props
        /// <summary>
        /// Gets the loaded plugin.
        /// </summary>
        public static PluginSystem.Plugins Plugins
        {
            get
            {
                if (_plugins == null)
                    throw new Exception($"The plugin system has not been set.  Please invoke the '{nameof(SetPlugins)}'() method to set the plugin system.");


                return _plugins;
            }
        }
        #endregion


        #region Props
        /// <summary>
        /// Sets the loaded plugins for use.
        /// </summary>
        /// <param name="plugins">The plugins to apply to the <see cref="CorePluginSystem"/>.</param>
        public static void SetPlugins(PluginSystem.Plugins plugins) => _plugins = plugins;


        /// <summary>
        /// Clears any loaded plugins.
        /// </summary>
        public static void ClearPlugins() => _plugins = null;
        #endregion
    }
}
