using ScorpionCore.Plugins;
using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using ScorpionCore.Exceptions;

namespace ScorpionCore
{
    /// <summary>
    /// Loads plugin assemblies for use.
    /// </summary>
    public static class PluginLoader
    {
        private static Assembly _pluginAssembly;
        private static Dictionary<string, string> _pluginNames = new Dictionary<string, string>();
        private const string PLUGIN_NAMESPACE = "ScorpionCore.Plugins";
        private static string _pluginAssemblyName;


        /// <summary>
        /// Loads the plugin assembly for use.
        /// </summary>
        /// <param name="pluginLibraryName">The name of the plugin assembly.</param>
        public static void LoadPlugin(string pluginLibraryName)
        {
            //If the plugin has already been loaded, just exit
            if (_pluginAssembly != null)
                return;

            _pluginAssemblyName = pluginLibraryName;

            var pluginPath = AppDomain.CurrentDomain.BaseDirectory;

            var dirInfo = new DirectoryInfo(pluginPath);

            var pluginAssemblyFileName = dirInfo.GetFiles()
                .Where(f => f.Name.ToLower().Contains($"{pluginLibraryName}.dll".ToLower()))
                .Select(f => f.FullName).ToArray().FirstOrDefault();

            _pluginAssembly = Assembly.LoadFrom(pluginAssemblyFileName);

            ValidPluginInterfaces = BuildValidPluginInterfaceList();
        }


        /// <summary>
        /// Gets the list of valid plugin interfaces that exist in the plugin assembly.
        /// </summary>
        public static string[] ValidPluginInterfaces { get; private set; }


        /// <summary>
        /// Gets the first plugin from the plugin assembly that matches type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of plugin to load.</typeparam>
        /// <returns></returns>
        public static T GetPluginByType<T>() where T : class
        {
            var type = typeof(T).Name;

            if (IsValidPluginName<T>())
            {
                var fullName = GetFullPluginName(SanitizeTypeName<T>());
                var pluginType = _pluginAssembly.GetType(fullName);

                //TODO: Create custom exception class for this exception
                if (pluginType == null)
                    throw new PluginNotFoundException(fullName, _pluginAssemblyName);

                var plugin = Activator.CreateInstance(pluginType) as T;

                //Check to make sure that the instance implements the IPlugin interface
                if (!(plugin is IPlugin))
                    throw new PluginMustImplementInterfaceException(plugin.GetType().Name);

                if (plugin == null)
                    throw new PluginNotFoundException(fullName, _pluginAssemblyName);

                return plugin;
            }


            throw new Exception($"The requested plugin with the name '{type}' does not exist.");
        }


        /// <summary>
        /// Gets the first plugin from the plugin assembly that matches the given <paramref name="name"/>.
        /// </summary>
        /// <typeparam name="T">The plugin type.</typeparam>
        /// <param name="name">The name of the plugin.</param>
        /// <returns></returns>
        public static T GetPluginByName<T>(string name) where T : class
        {
            if (IsValidPluginName<T>())
            {
                var fullName = GetFullPluginName(SanitizeTypeName(name));
                var pluginType = _pluginAssembly.GetType(fullName);
                var exceptionMessage = $"The plugin '{fullName}' could not be found in the plugin assembly '{_pluginAssemblyName}.dll' or does not implement from proper plugin interface or the interface {typeof(T).Name} is not a valid plugin interface.";

                if (pluginType == null)
                    throw new Exception(exceptionMessage);

                var plugin = Activator.CreateInstance(pluginType) as T;

                //Check to make sure that the instance implements the IPlugin interface
                if (!(plugin is IPlugin))
                    throw new PluginMustImplementInterfaceException(plugin.GetType().Name);

                if (plugin == null)
                    throw new Exception(exceptionMessage);

                return plugin;
            }


            throw new Exception($"The requested plugin with the name '{name}' does not exist.");
        }


        /// <summary>
        /// Returns the names of available plugins that match the given type.
        /// </summary>
        /// <param name="type">The type of plugin to check for.  This is case sensitive.</param>
        /// <returns></returns>
        public static string[] GetPluginNamesThatMatchType<T>()
        {
            //TODO: Change exception message to tell user what the plugin name requirments are.
            if (!IsValidPluginName<T>())
                throw new Exception($"");

            //Get a list of all the possible plugin names that match the given type.
            return (from p in _pluginAssembly.GetExportedTypes()
                    where p.Name.Contains(SanitizeTypeName<T>())
                    select p.FullName.Replace($"{_pluginAssemblyName}.", "")).ToArray();
        }


        /// <summary>
        /// Returns the names of available plugins that match the given type.
        /// </summary>
        /// <param name="pluginType">The type of plugin to check for.  Must be an interface and match the list of <see cref="PluginLoader.ValidPluginInterfaces"/>.</param>
        /// <returns></returns>
        public static string[] GetPluginNamesThatMatchType(Type pluginType)
        {
            //TODO: Change exception message to tell user what the plugin name requirments are.
            if (!IsValidPluginName(pluginType))
                throw new Exception($"");

            var typeName = pluginType.Name;

            //Remove the letter 'I' from the begining of the type name
            typeName = typeName.Substring(1, typeName.Length - 1);

            //Get a list of all the possible plugin names that match the given type.
            return (from p in _pluginAssembly.GetExportedTypes()
                    where p.Name.Contains(typeName)
                    select p.FullName.Replace($"{_pluginAssemblyName}.", "")).ToArray();
        }


        #region Private Methods
        /// <summary>
        /// Gets the actual name of the plugin in the loaded plugin assembly
        /// that matches the requirements.
        /// </summary>
        /// <param name="subName">The sub name that must exist in the plugin assembly.</param>
        /// <returns></returns>
        private static string GetFullPluginName(string subName)
        {
            var myTypes = _pluginAssembly.GetExportedTypes();

            foreach (var plugin in _pluginAssembly.GetExportedTypes())
            {
                if (plugin.Name.Contains(subName))
                {
                    var stop = true;
                }
            }
            //Get a list of all the exported types that could be a valid plugin
            var possiblePlugins = (from p in _pluginAssembly.GetExportedTypes()
                                   where p.Name.Contains(subName)
                                   select p.FullName).ToArray();

            //Throw an exception if there are no plugins available or if there are more then one
            if (possiblePlugins.Length <= 0)
            {
                var message = $@"No plugins with the name '{subName}' exist in the plugin assmbly";
                throw new Exception(message);
            }


            return possiblePlugins[0];
        }


        /// <summary>
        /// Builds a list of valid plugin names.
        /// </summary>
        /// <returns></returns>
        private static string[] BuildValidPluginInterfaceList()
        {
            //Return all of the namespace names as long as they are interfaces,
            //have a name that is at least 2 characters, starts with the convention
            //letter I, and is in the plugin interface. Removes the letter 'I' from
            //each plugin name

            var type = (from t in Assembly.GetExecutingAssembly().GetTypes()
                        where t.Name.Contains("Renderer")
                        select t).FirstOrDefault();

            return (from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsInterface &&
                        t.Name.Length >= 2 &&
                        t.Name[0] == 'I' &&
                        t.Namespace == PLUGIN_NAMESPACE && //From the plugin namespace
                        t.GetInterfaces().Any(i => i.Name == nameof(IPlugin)) //Is a plugin
                    select t.Name).ToArray();
        }


        /// <summary>
        /// Returns true if the given generic type param <typeparamref name="T"/> is a valid plugin name.
        /// </summary>
        /// <returns></returns>
        private static bool IsValidPluginName<T>()
        {
            //TODO: Test that the type is of type IPlugin.
            
            var incomingType = typeof(T);

            var typeName = incomingType.Name;

            //If the incoming type is not an interface
            if (!incomingType.IsInterface)
                return false;

            //The incoming interface type must start with the letter 'I'
            if (typeName.Length >= 2 && typeName.Substring(0, 1) != "I")
                return false;


            //Check to see if the given plugin name exists in the valid plugin interface list.
            return ValidPluginInterfaces.Any(p => p == typeName);
        }


        /// <summary>
        /// Returns true if the given <paramref name="pluginType"/> is a valid plugin name.
        /// </summary>
        /// <param name="pluginType">The plugin type to check.</param>
        /// <returns></returns>
        private static bool IsValidPluginName(Type pluginType)
        {
            var typeName = pluginType.Name;

            //If the incoming type is not an interface
            if (!pluginType.IsInterface)
                return false;

            //The incoming interface type must start with the letter 'I'
            if (typeName.Length >= 2 && typeName.Substring(0, 1) != "I")
                return false;


            //Check to see if the given plugin name exists in the valid plugin interface list.
            return ValidPluginInterfaces.Any(p => p == typeName);
        }


        /// <summary>
        /// Santizes the incoming type of type <typeparamref name="T"/> by removing the letter 'I'
        /// if it starts with the letter 'I'.
        /// </summary>
        /// <typeparam name="T">The type to sanitize.</typeparam>
        /// <returns></returns>
        private static string SanitizeTypeName<T>()
        {
            var typeName = typeof(T).Name;

            return SanitizeTypeName(typeof(T).Name);
        }


        /// <summary>
        /// Santizes the incoming type of type <typeparamref name="T"/> by removing the letter 'I'
        /// if it starts with the letter 'I'.
        /// </summary>
        /// <param name="name">The name to sanitize.</param>
        /// <returns></returns>
        private static string SanitizeTypeName(string name)
        {
            //If the incoming type starts with the letter 'I'
            if (!string.IsNullOrEmpty(name) && name.Length >= 2)
            {
                if(name[0] == 'I')
                    return name.Substring(1, name.Length - 1);

                return name;
            }


            throw new ArgumentException(nameof(name), $"The type name of {name} is invalid.");
        }
        #endregion
    }
}
