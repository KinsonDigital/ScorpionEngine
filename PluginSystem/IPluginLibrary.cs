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
        /// Loads a concrete plugin that implements the <see cref="IPlugin"/> interface.
        /// </summary>
        /// <typeparam name="T">The type of plugin to load.  Must implement the <see cref="IPlugin"/> interface.</typeparam>
        /// <returns></returns>
        T LoadPlugin<T>() where T : class, IPlugin;


        /// <summary>
        /// Loads a concrete plugin that implements the <see cref="IPlugin"/> interface and sends in the
        /// given <paramref name="paramItems"/> values when instantiating the plugin.
        /// </summary>
        /// <typeparam name="T">The type of plugin to load.  Must implement the <see cref="IPlugin"/> interface.</typeparam>
        /// <param name="paramItems">The list of parameters to send into the plugin.</param>
        /// <returns></returns>
        T LoadPlugin<T>(params object[] paramItems) where T : class, IPlugin;
        #endregion
    }
}