using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using PluginSystem.Exceptions;

[assembly: InternalsVisibleTo(assemblyName: "ScorpionCoreTests", AllInternalsVisible = true)]
[assembly: InternalsVisibleTo(assemblyName: "ScorpionEngineTests", AllInternalsVisible = true)]
[assembly: InternalsVisibleTo(assemblyName: "ScorpionUITests", AllInternalsVisible = true)]
[assembly: InternalsVisibleTo(assemblyName: "ScorpionEngine", AllInternalsVisible = true)]

namespace PluginSystem
{
    /// <summary>
    /// Loads plugin assemblies for use.
    /// </summary>
    [ExcludeFromCodeCoverage]
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
    }
}
