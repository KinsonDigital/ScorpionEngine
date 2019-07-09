using KDScorpionCore.Plugins;
using System.Diagnostics.CodeAnalysis;

namespace KDScorpionCore
{
    /// <summary>
    /// Loads up and boostraps the <see cref="CorePluginSystem"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
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
