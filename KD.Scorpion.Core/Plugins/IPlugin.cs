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
        /// Gets the data as the given type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="option">Used to pass in options for the <see cref="GetData{T}(int)"/> implementation to process.</param>
        /// <typeparam name="T">The type of data to get.</typeparam>
        /// <returns></returns>
        T GetData<T>(int option) where T : class;
    }
}
