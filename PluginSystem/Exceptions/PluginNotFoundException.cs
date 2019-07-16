using System;

namespace PluginSystem.Exceptions
{
    /// <summary>
    /// Thrown when a plugin cannot be found.
    /// </summary>
    public class PluginNotFoundException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="PluginNotFoundException"/>.
        /// </summary>
        public PluginNotFoundException() :base("The plugin could not be found in the plugin assembly") { }


        /// <summary>
        /// Creates a new instance of <see cref="PluginNotFoundException"/>.
        /// </summary>
        /// <param name="pluginName">The name of the plugin that was not found.</param>
        /// <param name="pluginAssembly">The name of the plugin assembly without the '.dll' extension.</param>
        public PluginNotFoundException(string pluginName, string pluginAssembly)
            : base($"The plugin '{pluginName}' could not be found in the plugin assembly '{pluginAssembly}.dll'.") { }
        #endregion
    }
}
