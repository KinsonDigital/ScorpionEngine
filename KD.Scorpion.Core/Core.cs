using KDScorpionCore.Plugins;

namespace KDScorpionCore
{
    /// <summary>
    /// Loads up and boostraps the core plugin system.
    /// </summary>
    internal static class Core
    {
        #region Public Methods
        /// <summary>
        /// Starts up the core.
        /// </summary>
        /// <returns></returns>
        public static IEngineCore Start()
        {
            var plugins = new PluginSystem.Plugins();
            CorePluginSystem.SetPlugins(plugins);


            return CorePluginSystem.Plugins.EnginePlugins.LoadPlugin<IEngineCore>();
        }
        #endregion
    }
}
