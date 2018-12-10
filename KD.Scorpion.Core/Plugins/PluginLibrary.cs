using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace KDScorpionCore.Plugins
{
    [ExcludeFromCodeCoverage]
    public class PluginLibrary : IPluginLibrary
    {
        private Assembly _pluginAssembly;
        private Container _container;
        private IEnumerable<Type> _concretePluginTypes;


        public PluginLibrary(string name)
        {
            Name = name;

            _container = new Container();

            //Load the plugin assembly.
            _pluginAssembly = PluginLibraryLoader.LoadPluginLibrary(name);

            //Get all of the compatible plugin types to register
            _concretePluginTypes = _container.GetTypesToRegister<IPlugin>(_pluginAssembly);

            //Register all of the plugin types with the IoC container for instantiation
            foreach (var concreteType in _concretePluginTypes)
            {
                var serviceInterface = GetPluginInterface(concreteType);

                //If the concrete type is valid for registration
                if(ValidForRegistration(concreteType))
                    _container.Register(serviceInterface, concreteType);
            }
        }


        #region Props
        public string Name { get; set; }
        #endregion


        #region Public Methods
        public T LoadPlugin<T>() where T : class, IPlugin
        {
            return _container.GetInstance<T>();
        }


        public T LoadPlugin<T>(params object[] paramItems) where T : class, IPlugin
        {
            var foundConcreteType = (from p in _concretePluginTypes
                                     where p.GetInterfaces().Any(i => i.Name == typeof(T).Name)
                                     select p).FirstOrDefault();

            return Activator.CreateInstance(foundConcreteType, paramItems) as T;
        }
        #endregion


        #region Private Methods
        private Type GetPluginInterface(Type concreteType)
        {
            return (from i in concreteType.GetInterfaces()
                    where i.Name != nameof(IPlugin) &&
                        i.GetInterfaces().Any(interfaceType => interfaceType.Name == nameof(IPlugin))
                    select i).FirstOrDefault();
        }


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
