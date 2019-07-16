using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
using PluginSystem.Exceptions;

namespace PluginSystem
{
    /// <summary>
    /// Loads plugin assemblies for use.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class PluginLibraryLoader
    {
        #region Public Methods
        /// <summary>
        /// Loads the plugin assembly for use.
        /// </summary>
        /// <param name="pluginLibraryName">The name of the plugin assembly.</param>
        /// <remarks>The <paramref name="pluginLibraryName"/> param can be with or without the '.dll' extension.</remarks>
        public static Assembly LoadPluginLibrary(string pluginLibraryName)
        {
            var pluginPath = AppDomain.CurrentDomain.BaseDirectory;

            //If the name of the library contains the file extension, remove it
            pluginLibraryName = pluginLibraryName.EndsWith(".dll") ? pluginLibraryName.Replace(".dll", "") : pluginLibraryName;

            var dirInfo = new DirectoryInfo(pluginPath);

            var pluginAssemblyFileName = dirInfo.GetFiles()
                .Where(f => f.Name.ToLower().Contains($"{pluginLibraryName}.dll".ToLower()))
                .Select(f => f.FullName).ToArray().FirstOrDefault();

            Assembly pluginAssembly;

            try
            {
                pluginAssembly = Assembly.LoadFrom(pluginAssemblyFileName);
            }
            catch (Exception ex)
            {
                if(ex is FileNotFoundException || ex is ArgumentNullException)
                    throw new PluginNotFoundException(pluginLibraryName, $"{pluginLibraryName}");

                return null;
            }


            return pluginAssembly;
        }
        #endregion
    }
}
