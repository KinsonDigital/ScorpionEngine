using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace ParticleMaker.Services
{
    /// <summary>
    /// Creates JSON files with data.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class JSONFileService : IFileService
    {
        #region Public Methods
        /// <summary>
        /// Creates a JSON file at the given <paramref name="path"/> with the given <paramref name="data"/>.
        /// </summary>
        /// <typeparam name="T">THe type of data to save.</typeparam>
        /// <param name="path">The directory path to the file.</param>
        /// <param name="data">The data to save in the file.</param>
        public void Create<T>(string path, T data) where T : class
        {
            try
            {
                var fileData = JsonConvert.SerializeObject(data);

                using (var file = File.CreateText(path))
                {
                    file.Write(fileData);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Loads a file at the given <paramref name="path"/>.
        /// </summary>
        /// <typeparam name="T">The type of data to load from the file.</typeparam>
        /// <param name="path">The directory path to the file.</param>
        /// <returns>The data of type <typeparamref name="T"/>.</returns>
        public T Load<T>(string path) where T : class
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
