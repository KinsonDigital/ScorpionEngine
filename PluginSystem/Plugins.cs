using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(assemblyName: "ScorpionCoreTests", AllInternalsVisible = true)]
[assembly: InternalsVisibleTo(assemblyName: "ScorpionEngineTests", AllInternalsVisible = true)]

namespace PluginSystem
{
    public static class Plugins
    {
        #region Props
        /// <summary>
        /// The loaded plugin factory.
        /// </summary>
        public static IPluginFactory PluginFactory { get; private set; }
        #endregion


        #region Public Methods
        public static void LoadPluginFactory()
        {
#if MONOGAME
            PluginFactory = new MonoPluginFactory();
#elif SDL
            PluginFactory = new SDLPluginFactory();
#endif
        }


        /// <summary>
        /// Unloads the currently loaded plugin factory.
        /// </summary>
        public static void UnloadPluginFactory() => PluginFactory = null;
        #endregion


        #region Internal Methods
        /// <summary>
        /// Loads a plugin factory.  This is used for unit testing only.
        /// </summary>
        /// <param name="factory">The plugin factory to load.</param>
        internal static void LoadPluginFactory(IPluginFactory factory) => PluginFactory = factory;
        #endregion
    }   
}
