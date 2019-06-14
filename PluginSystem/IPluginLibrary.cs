namespace PluginSystem
{
     /// <summary>
    /// Represents a library with various plugins of functionality that can be loaded and used 
    /// in an application.
    /// </summary>
    public interface IPluginLibrary
    {
        #region Props
        /// <summary>
        /// Gets the name of the plugin assembly.
        /// </summary>
        string Name { get; }
        #endregion


        #region Methods
        /// <summary>
        /// Loads the concrete plugin that matches the given type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of plugin to load.</typeparam>
        /// <returns></returns>
        T LoadPlugin<T>() where T : class, IPlugin;


        /// <summary>
        /// Loads the concrete plugins that matche the given type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of plugins to load.</typeparam>
        /// <param name="paramItems"></param>
        /// <returns></returns>
        T LoadPlugin<T>(params object[] paramItems) where T : class, IPlugin;
        #endregion
    }
}