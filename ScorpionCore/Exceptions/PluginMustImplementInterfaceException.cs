using ScorpionCore.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionCore.Exceptions
{
    /// <summary>
    /// Thrown when a plugin does not implement the <see cref="Plugins.IPlugin"/> interface.
    /// </summary>
    public class PluginMustImplementInterfaceException : Exception
    {
        /// <summary>
        /// Creates a new isntance of <see cref="PluginMustImplementInterfaceException"/>.
        /// </summary>
        public PluginMustImplementInterfaceException()
            : base($"The plugin must implement the {nameof(IPlugin)} interface.")
        {
        }


        /// <summary>
        /// Creates a new isntance of <see cref="PluginMustImplementInterfaceException"/>.
        /// </summary>
        /// <param name="pluginName">The name of the plugin.</param>
        public PluginMustImplementInterfaceException(string pluginName)
            : base($"The plugin {pluginName} must implement the {nameof(IPlugin)} interface.")
        {
        }
    }
}
