using ScorpionCore;
using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace ScorpionEngine.Config
{
    public static class PluginLoader
    {
        private static Assembly _pluginAssembly;
        private static Dictionary<string, string> _pluginNames = new Dictionary<string, string>();


        public static void Init()
        {
            var pluginPath = AppDomain.CurrentDomain.BaseDirectory;

            var dirInfo = new DirectoryInfo(pluginPath);

            var pluginAssemblyFileName = dirInfo.GetFiles()
                .Where(f => f.Name.ToLower().Contains("plugin"))
                .Select(f => f.FullName).ToArray().FirstOrDefault();

            _pluginAssembly = Assembly.LoadFrom(pluginAssemblyFileName);

            SetupPluginNames();
        }

        private static void SetupPluginNames()
        {
            _pluginNames.Add("ContentLoader", "");
            _pluginNames.Add("EngineCore", "");
            _pluginNames.Add("EngineTime", "");
            _pluginNames.Add("Keyboard", "");
            _pluginNames.Add("Mouse", "");
            _pluginNames.Add("Renderer", "");
            _pluginNames.Add("Text", "");
            _pluginNames.Add("Texture", "");

            var keys = _pluginNames.Keys.ToArray();

            for (int i = 0; i < keys.Length; i++)
            {
                var currentKey = keys[i].ToLower();

                _pluginNames[keys[i]] = (from p in _pluginAssembly.ExportedTypes
                                     where p.Name.ToLower().Contains(keys[i].ToLower())
                                     select p.FullName).FirstOrDefault();
            }
        }


        public static IContentLoader GetContentLoader()
        {
            //The key value that must exist in the plugin name to be successfully loaded
            var pluginKey = "ContentLoader";

            if (PluginExists(pluginKey))
            {
                var pluginName = GetPluginName(pluginKey);

                var contentLoaderType = _pluginAssembly.GetType(pluginName);

                var contentLoader = Activator.CreateInstance(contentLoaderType);

                return contentLoader as IContentLoader;
            }


            throw new Exception($"The plugin {pluginKey} does not exist in the plugin assembly.");
        }


        public static IEngineCore GetEngineCore()
        {
            //The key value that must exist in the plugin name to be successfully loaded
            var pluginKey = "EngineCore";

            if (PluginExists(pluginKey))
            {
                var pluginName = GetPluginName(pluginKey);

                var contentLoaderType = _pluginAssembly.GetType(pluginName);

                var contentLoader = Activator.CreateInstance(contentLoaderType);

                return contentLoader as IEngineCore;
            }


            throw new Exception($"The plugin {pluginKey} does not exist in the plugin assembly.");
        }


        public static IRenderer GetRenderer()
        {
            //The key value that must exist in the plugin name to be successfully loaded
            var pluginKey = "Renderer";

            if (PluginExists(pluginKey))
            {
                var pluginName = GetPluginName(pluginKey);

                var contentLoaderType = _pluginAssembly.GetType(pluginName);

                var contentLoader = Activator.CreateInstance(contentLoaderType);

                return contentLoader as IRenderer;
            }


            throw new Exception($"The plugin {pluginKey} does not exist in the plugin assembly.");
        }


        private static bool PluginExists(string key)
        {
            var fullPluginName = _pluginNames[key];

            return _pluginAssembly.ExportedTypes.Any(plugin => plugin.FullName.ToLower().Contains(fullPluginName.ToLower()));
        }


        private static string GetPluginName(string key)
        {
            return _pluginNames[key];
        }
    }
}
