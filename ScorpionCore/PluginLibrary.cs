using ScorpionCore.Exceptions;
using ScorpionCore.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionCore
{
    public class PluginLibrary
    {
        private Assembly _pluginAssembly;
        private Dictionary<string, string> _pluginNames = new Dictionary<string, string>();
        private const string PLUGIN_NAMESPACE = "ScorpionCore.Plugins";
        private string[] _validPluginInterfaces;

        public string PluginAssemblyName { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="PluginLibrary"/>.
        /// </summary>
        /// <param name="pluginAssembly"></param>
        internal PluginLibrary(Assembly pluginAssembly)
        {
            _pluginAssembly = pluginAssembly;
            PluginAssemblyName = _pluginAssembly.GetName().Name;

            _validPluginInterfaces = GetPluginInterfaces();
            ValidPlugins = GetValidPlugins();
        }


        /// <summary>
        /// Gets the list of valid plugin interfaces that exist in the plugin assembly.
        /// </summary>
        public PluginInfo[] ValidPlugins { get; private set; }


        /// <summary>
        /// Gets the first plugin from the plugin assembly that matches type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of plugin to load.</typeparam>
        /// <returns></returns>
        public T GetPluginByType<T>() where T : class
        {
            var typeName = typeof(T).Name;

            if (IsValidPluginName<T>())
            {
                var foundPlugin = (from pluginInfo in ValidPlugins
                                where pluginInfo.PluginInterfaceName == typeName
                                select pluginInfo).FirstOrDefault();

                var pluginType = _pluginAssembly.GetType(foundPlugin.FullName);

                if (pluginType == null)
                    throw new PluginNotFoundException(foundPlugin.FullName, PluginAssemblyName);

                //Check for a parameter less constructor
                //If the plugin does not have a valid constructor
                if (NoValidPluginConstructor(pluginType))
                    throw new Exception($"The plugin {foundPlugin.Name} does not have a parameterless constructor");

                var plugin = Activator.CreateInstance(pluginType) as T;

                //Check to make sure that the instance implements the IPlugin interface
                if (!(plugin is IPlugin))
                    throw new PluginMustImplementInterfaceException(plugin.GetType().Name);

                if (plugin == null)
                    throw new PluginNotFoundException(foundPlugin.FullName, PluginAssemblyName);

                return plugin;
            }


            throw new Exception($"The requested plugin with the name '{typeName}' does not exist.");
        }


        public T GetPluginByType<T>(object[] constructorParams) where T : class
        {
            var typeName = typeof(T).Name;

            if (IsValidPluginName<T>())
            {
                var foundPlugin = (from pluginInfo in ValidPlugins
                                   where pluginInfo.PluginInterfaceName == typeName
                                   select pluginInfo).FirstOrDefault();

                var pluginType = _pluginAssembly.GetType(foundPlugin.FullName);

                if (pluginType == null)
                    throw new PluginNotFoundException(foundPlugin.FullName, PluginAssemblyName);

                if (!HasValidConstructor(pluginType, constructorParams.Select(c => c.GetType()).ToArray()))
                    throw new ArgumentException($"Invalid plugin constructor params", nameof(constructorParams));

                //Check for a parameter less constructor
                //If the plugin does not have a valid constructor
                if (NoValidPluginConstructor(pluginType))
                    throw new Exception($"The plugin {foundPlugin.Name} does not have a parameterless constructor");

                var ctors = pluginType.GetConstructors();

                var myParams = ctors[1].GetParameters();

                var plugin = Activator.CreateInstance(pluginType, constructorParams) as T;

                //Check to make sure that the instance implements the IPlugin interface
                if (!(plugin is IPlugin))
                    throw new PluginMustImplementInterfaceException(plugin.GetType().Name);

                if (plugin == null)
                    throw new PluginNotFoundException(foundPlugin.FullName, PluginAssemblyName);

                return plugin;
            }


            throw new Exception($"The requested plugin with the name '{typeName}' does not exist.");
        }


        /// <summary>
        /// Gets the first plugin from the plugin assembly that matches the given <paramref name="name"/>.
        /// </summary>
        /// <typeparam name="T">The plugin type.</typeparam>
        /// <param name="name">The name of the plugin.</param>
        /// <returns></returns>
        public T GetPluginByName<T>(string name) where T : class
        {
            var typeName = typeof(T).Name;

            if (IsValidPluginName<T>())
            {
                var foundPlugin = (from pluginInfo in ValidPlugins
                                   where pluginInfo.PluginInterfaceName == typeName &&
                                        pluginInfo.Name == name
                                   select pluginInfo).FirstOrDefault();

                var pluginType = _pluginAssembly.GetType(foundPlugin.FullName);
                var exceptionMessage = $"The plugin '{foundPlugin.FullName}' could not be found in the plugin assembly '{PluginAssemblyName}.dll' or does not implement from proper plugin interface or the interface {typeof(T).Name} is not a valid plugin interface.";

                if (pluginType == null)
                    throw new Exception(exceptionMessage);

                //If the plugin does not have a valid constructor
                if (NoValidPluginConstructor(pluginType))
                    throw new Exception("The plugin does not have a parameter constructor");

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
        public string[] GetPluginNamesThatMatchType<T>()
        {
            //TODO: Change exception message to tell user what the plugin name requirments are.
            if (!IsValidPluginName<T>())
                throw new Exception($"");

            //Get a list of all the possible plugin names that match the given type.
            return (from p in _pluginAssembly.GetExportedTypes()
                    where p.Name.Contains(SanitizeTypeName<T>())
                    select p.FullName.Replace($"{PluginAssemblyName}.", "")).ToArray();
        }


        /// <summary>
        /// Returns the names of available plugins that match the given type.
        /// </summary>
        /// <param name="pluginType">The type of plugin to check for.  Must be an interface and match the list of <see cref="PluginLoader.ValidPluginInterfaces"/>.</param>
        /// <returns></returns>
        public string[] GetPluginNamesThatMatchType(Type pluginType)
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
                    select p.FullName.Replace($"{PluginAssemblyName}.", "")).ToArray();
        }


        #region Private Methods
        /// <summary>
        /// Gets the actual name of the plugin in the loaded plugin assembly
        /// that matches the requirements.
        /// </summary>
        /// <param name="subName">The sub name that must exist in the plugin assembly.</param>
        /// <returns></returns>
        private string GetFullPluginName(string subName)
        {
            var myTypes = _pluginAssembly.GetExportedTypes();

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
        /// Creates a list of valid plugins.
        /// </summary>
        /// <returns></returns>
        private PluginInfo[] GetValidPlugins()
        {
            //Return all of the namespace names as long as they are interfaces,
            //have a name that is at least 2 characters, starts with the convention
            //letter I, and is in the plugin interface. Removes the letter 'I' from
            //each plugin name

            return (from t in _pluginAssembly.GetTypes()
                    where t.Namespace == PluginAssemblyName && //From the plugin namespace
                        t.GetInterfaces().Any(i => i.Name == nameof(IPlugin)) //Is a plugin
                    select new PluginInfo()
                    {
                        Name = t.Name,
                        FullName = t.FullName,
                        PluginInterfaceName = GetValidTypeInterface(t)
                    }).ToArray();
        }


        private bool HasValidConstructor(Type pluginType, Type[] paramTypes)
        {
            //If the plugin type has no construcotrs
            if (pluginType.GetConstructors().Length <= 0)
                return false;

            var constructors = pluginType.GetConstructors();

            //Get the list of constructors with the proper amount of params
            var validCtrs = (from c in constructors
                             where c.GetParameters().Length == paramTypes.Length
                             select c)
                            .Where(c =>
                            {
                                var ctorParams = c.GetParameters();

                                var allSame = true;

                                for (int i = 0; i < paramTypes.Length; i++)
                                {
                                    if(!BothImplementSameInterface(ctorParams[i].ParameterType, paramTypes[i]))
                                    {
                                        allSame = false;
                                        break;
                                    }
                                    ////If the required ctor type is an interface, compare
                                    ////if the incoming param implements that interface. If not,
                                    ////just compare if the 2 types are the same
                                    //if (ctorParams[i].ParameterType.IsInterface)
                                    //{
                                    //    var paramInterfaces = paramTypes[i].GetInterfaces();

                                    //    var paramInterface = paramInterfaces.Length > 0 ?
                                    //        paramInterfaces[0] :
                                    //        throw new Exception($"The param {paramTypes[i].Name} must implement constructor param interface of {ctorParams[i].ParameterType.GetInterfaces()[0].Name}.");

                                    //    if (!ctorParams[i].ParameterType.Equals(paramInterface))
                                    //        allSame = false;
                                    //}
                                    //else
                                    //{
                                    //    if (!ctorParams[i].ParameterType.Equals(paramTypes[i]))
                                    //        allSame = false;
                                    //}
                                }

                                return allSame;
                            }).ToArray();
                             

            //If there are not constructors that have the required number of params
            if (validCtrs.Length <= 0)
                return false;


            return true;
        }


        private bool IsSameType(Type typeA, Type typeB)
        {
            //First check if both types are not arrays. Return false if they are both not
            if(typeA.IsArray && typeB.IsArray)
            {
                //Get the element type of both params
                var typeAElementType = typeA.GetElementType();
                var typeBElementType = typeB.GetElementType();

                //If both element types are the same
                if(typeAElementType == typeBElementType)
                {
                    return true;
                }
                else
                {
                    //If both array element types implement an interface
                    return BothImplementSameInterface(typeA, typeB);
                }
            }


            return false;
        }


        private bool BothImplementInterface(Type typeA, Type typeB)
        {
            return typeA.GetInterfaces().Length > 0 && typeB.GetInterfaces().Length > 0;
        }


        private bool BothImplementSameInterface(Type typeA, Type typeB)
        {
            if(BothImplementInterface(typeA, typeB))
            {
                var typeAInterfaces = (from i in typeA.GetInterfaces() select i.Name).ToArray();
                var typeBInterfaces = (from i in typeB.GetInterfaces() select i.Name).ToArray();

                return typeAInterfaces.Intersect(typeBInterfaces).Any();
            }


            return false;
        }


        private bool NoValidPluginConstructor(Type pluginType)
        {
            var constructors = pluginType.GetConstructors();

            if (constructors.Length <= 0)
                return true;

            return constructors.All(c => c.GetParameters().Length > 0);
        }


        //TODO: Add docs
        private string GetValidTypeInterface(Type pluginType)
        {
            var interfaces = pluginType.GetInterfaces();

            if (interfaces == null || interfaces.Length <= 0)
                return "";

            return interfaces.Where(i =>
            {
                return _validPluginInterfaces.Contains(i.Name) && i.Name != nameof(IPlugin);
            }).FirstOrDefault().Name;
        }


        //TODO: Add docs
        private string[] GetPluginInterfaces()
        {
            return (from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.Namespace == PLUGIN_NAMESPACE &&
                        t.GetInterfaces().Any(i => i.Name == nameof(IPlugin))
                    select t.Name).ToArray();
        }
        

        /// <summary>
        /// Returns true if the given generic type param <typeparamref name="T"/> is a valid plugin name.
        /// </summary>
        /// <returns></returns>
        private bool IsValidPluginName<T>()
        {
            var incomingType = typeof(T);

            var typeName = incomingType.Name;

            //If the incoming type is not an interface
            if (!incomingType.IsInterface)
                return false;

            //The incoming interface type must start with the letter 'I'
            if (typeName.Length <= 2)
                return false;

            //Check the name against the interface that the type implements


            //Check to see if the given plugin name exists in the valid plugin interface list.
            return ValidPlugins.Any(pluginInfo => pluginInfo.PluginInterfaceName == typeName);
        }


        /// <summary>
        /// Returns true if the given <paramref name="pluginType"/> is a valid plugin name.
        /// </summary>
        /// <param name="pluginType">The plugin type to check.</param>
        /// <returns></returns>
        private bool IsValidPluginName(Type pluginType)
        {
            var typeName = pluginType.Name;

            //If the incoming type is not an interface
            if (!pluginType.IsInterface)
                return false;

            //The incoming interface type must start with the letter 'I'
            if (typeName.Length >= 2 && typeName.Substring(0, 1) != "I")
                return false;


            //Check to see if the given plugin name exists in the valid plugin interface list.
            return ValidPlugins.Any(pluginInfo => pluginInfo.Name == typeName);
        }


        /// <summary>
        /// Santizes the incoming type of type <typeparamref name="T"/> by removing the letter 'I'
        /// if it starts with the letter 'I'.
        /// </summary>
        /// <typeparam name="T">The type to sanitize.</typeparam>
        /// <returns></returns>
        private string SanitizeTypeName<T>()
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
        private string SanitizeTypeName(string name)
        {
            //If the incoming type starts with the letter 'I'
            if (!string.IsNullOrEmpty(name) && name.Length >= 2)
            {
                if (name[0] == 'I')
                    return name.Substring(1, name.Length - 1);

                return name;
            }


            throw new ArgumentException(nameof(name), $"The type name of {name} is invalid.");
        }
        #endregion
    }
}
