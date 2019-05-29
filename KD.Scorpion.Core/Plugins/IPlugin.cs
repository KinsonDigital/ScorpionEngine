using System;

namespace KDScorpionCore.Plugins
{
    /// <summary>
    /// Represents a single plugin item.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Injects any arbitrary data into the plugin for use.  Must be a class.
        /// </summary>
        /// <typeparam name="T">The type of data to inject.</typeparam>
        /// <param name="data">The data to inject.</param>
        void InjectData<T>(T data) where T : class;


        /// <summary>
        /// Injects a pointer into the plugin for use.
        /// </summary>
        /// <param name="pointer"></param>
        void InjectPointer(IntPtr pointer);


        /// <summary>
        /// Gets any arbitrary data needed for use.
        /// </summary>
        /// <typeparam name="T">The type of data to get.</typeparam>
        /// <returns></returns>
        object GetData(string dataType);
    }
}
