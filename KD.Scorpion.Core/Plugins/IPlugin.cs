using System;

namespace KDScorpionCore.Plugins
{
    /// <summary>
    /// Represents a single plugin item.
    /// </summary>
    public interface IPlugin
    {
        //TODO: Improve method doc here explaining this method and the fact that it only injects class data
        /// <summary>
        /// Inject any arbitrary data into the plugin for use.
        /// </summary>
        /// <typeparam name="T">The type of data to inject.</typeparam>
        /// <param name="data">The data to inject.</param>
        void InjectData<T>(T data) where T : class;


        //TODO: Add method doc here explaining this method and the fact that it only injects struct data
        void InjectPointer(IntPtr pointer);


        /// <summary>
        /// Gets any arbitrary data needed for use.
        /// </summary>
        /// <typeparam name="T">The type of data to get.</typeparam>
        /// <returns></returns>
        object GetData(string dataType);
    }
}
