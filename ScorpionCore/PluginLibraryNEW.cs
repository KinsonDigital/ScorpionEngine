using ScorpionCore.Exceptions;
using ScorpionCore.Plugins;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionCore
{
    public class PluginLibraryNEW
    {
        private Assembly _pluginAssembly;
        private Container _container;


        public PluginLibraryNEW(string name)
        {
            _container = new Container();

            //Load the plugin assembly.
            _pluginAssembly = PluginLibraryLoaderNEW.LoadPluginLibrary(name);

            //Get all of the compatible plugin types to register
            var concretePluginTypes = _container.GetTypesToRegister<IPlugin>(_pluginAssembly);

            //Register all of the plugin types with the IoC container for instantiation
            foreach (var concreteType in concretePluginTypes)
            {
                var serviceInterface = GetPluginInterface(concreteType);

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
        #endregion


        #region Private Methods
        private Type GetPluginInterface(Type concreteType)
        {
            return (from i in concreteType.GetInterfaces()
                    where i.Name != nameof(IPlugin) &&
                        i.GetInterfaces().Any(interfaceType => interfaceType.Name == nameof(IPlugin))
                    select i).FirstOrDefault();
        }
        #endregion
    }
}
