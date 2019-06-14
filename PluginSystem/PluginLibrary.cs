using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using SimpleInjector;

namespace PluginSystem
{
    /// <summary>
    /// Represents a library with various plugins of functionality that can be loaded and used 
    /// in an application.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PluginLibrary : IPluginLibrary
    {
        #region Private Fields
        private readonly Assembly _pluginAssembly;
        private readonly Container _container = new Container();
        private IEnumerable<Type> _concretePluginTypes;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="PluginLibrary"/>.
        /// </summary>
        /// <param name="name">The name of the plugin library to load.</param>
        public PluginLibrary(string name)
        {
            Name = name;

            //Load the plugin assembly.
            _pluginAssembly = PluginLibraryLoader.LoadPluginLibrary(name);

            CheckAndRegisterAssembly();
        }


        /// <summary>
        /// Creates a new instance of <see cref="PluginLibrary"/>.
        /// </summary>
        /// <param name="pluginAssembly">The plugin assembly to load.</param>
        public PluginLibrary(Assembly pluginAssembly)
        {
            Name = pluginAssembly.GetName().Name;
            _pluginAssembly = pluginAssembly;
            CheckAndRegisterAssembly();
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets the name of the plugin assembly.
        /// </summary>
        public string Name { get; private set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Loads the concrete plugin that matches the given type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of plugin to load.</typeparam>
        /// <returns></returns>
        public T LoadPlugin<T>() where T : class, IPlugin => _container.GetInstance<T>();


        /// <summary>
        /// Loads the concrete plugins that matche the given type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of plugins to load.</typeparam>
        /// <param name="paramItems"></param>
        /// <returns></returns>
        public T LoadPlugin<T>(params object[] paramItems) where T : class, IPlugin
        {
            var foundConcreteType = (from p in _concretePluginTypes
                                     where p.GetInterfaces().Any(i => i.Name == typeof(T).Name)
                                     select p).FirstOrDefault();


            return Activator.CreateInstance(foundConcreteType, paramItems) as T;
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Checks and registers up the plugin assembly.
        /// </summary>
        private void CheckAndRegisterAssembly()
        {
            //Get all of the compatible plugin types to register
            _concretePluginTypes = _container.GetTypesToRegister<IPlugin>(_pluginAssembly);

            //Throw an exception if there are no concrete types in the plugin library to use.
            if (_concretePluginTypes.ToArray().Length <= 0)
            {
                //TODO: Create a better custom exception for the exception below
                throw new Exception($"The plugin library '{Name}' does not have any acceptable plugin classes to be loaded.");
            }

            //Register all of the plugin types with the IoC container for instantiation
            foreach (var concreteType in _concretePluginTypes)
            {
                var serviceInterface = GetPluginInterface(concreteType);

                //If the concrete type is valid for registration
                if (ValidForRegistration(concreteType))
                    _container.Register(serviceInterface, concreteType);
            }
        }


        /// <summary>
        /// Gets the interfce type that the given <paramref name="concreteType"/> implements.
        /// </summary>
        /// <param name="concreteType">The concrete type to pull the <see cref="IPlugin"/> interface from.</param>
        /// <returns></returns>
        private Type GetPluginInterface(Type concreteType)
        {
            return (from i in concreteType.GetInterfaces()
                    where i.Name != nameof(IPlugin) &&
                        i.GetInterfaces().Any(interfaceType => interfaceType.Name == nameof(IPlugin))
                    select i).FirstOrDefault();
        }


        /// <summary>
        /// Returns value indicating if the given <paramref name="type"/> is valid for IoC container registration.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns></returns>
        private bool ValidForRegistration(Type type)
        {
            //If the type has only one constructor and has
            //no value type parameters, it can be registered with the IoC container
            return TotalCtrs(type) == 1 && !CtrHasValueTypes(type);
        }


        /// <summary>
        /// Returns the total number of constructors.
        /// </summary>
        /// <param name="concreteType">The concreate type to check for the number of constructors on</param>
        /// <returns></returns>
        private int TotalCtrs(Type concreteType)
        {
            //If the type is an interface, then it definitly does not have any constructors
            if (concreteType.IsInterface)
                return 0;


            //Return total number of public and internal and protected constructors
            return concreteType.GetConstructors().Length + concreteType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).Length;
        }


        /// <summary>
        /// Returns a value indicating if the constructor of the given <paramref name="concreteType"/> has any value type parameters.
        /// </summary>
        /// <param name="concreteType">The concrete type to check.</param>
        /// <returns></returns>
        private bool CtrHasValueTypes(Type concreteType)
        {
            //If the type has no constructors, then it definitly does not have any value type parameters
            if(TotalCtrs(concreteType) == 0)
                return false;

            var ctrs = concreteType.GetConstructors();

            //Check each constructor for any value types
            foreach (var ctr in ctrs)
            {
                var ctrParams = ctr.GetParameters();

                if (ctrParams.Length == 0)
                    continue;

                //Check the parameters
                foreach (var param in ctrParams)
                {
                    if (param.ParameterType.IsValueType)
                        return true;
                }
            }


            return false;
        }
        #endregion
    }
}
