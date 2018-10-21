using ScorpionCore.Plugins;
using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using ScorpionCore.Exceptions;

namespace ScorpionCore.Plugins
{
    /// <summary>
    /// Loads plugin assemblies for use.
    /// </summary>
    internal static class PluginLibraryLoader
    {
        /// <summary>
        /// Loads the plugin assembly for use.
        /// </summary>
        /// <param name="pluginLibraryName">The name of the plugin assembly.</param>
        public static Assembly LoadPluginLibrary(string pluginLibraryName)
        {
            var pluginPath = AppDomain.CurrentDomain.BaseDirectory;

            var dirInfo = new DirectoryInfo(pluginPath);

            var pluginAssemblyFileName = dirInfo.GetFiles()
                .Where(f => f.Name.ToLower().Contains($"{pluginLibraryName}.dll".ToLower()))
                .Select(f => f.FullName).ToArray().FirstOrDefault();

            var pluginAssembly = Assembly.LoadFrom(pluginAssemblyFileName);


            return pluginAssembly;
        }
    }
}
