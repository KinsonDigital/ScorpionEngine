﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionCore.Exceptions
{
    /// <summary>
    /// Thrown when a plugin cannot be found.
    /// </summary>
    public class PluginNotFoundException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="PluginNotFoundException"/>.
        /// </summary>
        public PluginNotFoundException()
            :base("The plugin could not be found in the plugin assembly")
        {
        }


        /// <summary>
        /// Creates a new instance of <see cref="PluginNotFoundException"/>.
        /// </summary>
        public PluginNotFoundException(string pluginName, string pluginAssembly)
            : base($"The plugin '{pluginName}' could not be found in the plugin assembly '{pluginAssembly}.dll'.")
        {
        }
    }
}