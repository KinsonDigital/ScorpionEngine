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
        /// <typeparam name="T">The type of data to save.</typeparam>
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
                throw ex;
                //TODO: Properly handle exceptions
            }
        }


        /// <summary>
        /// Saves the given <paramref name="data"/> to the given <paramref name="path"/>.
        /// </summary>
        /// <typeparam name="T">The type of data to save into the file.</typeparam>
        /// <param name="path">The path of where to save the file.</param>
        /// <param name="data">The data to save in the file.</param>
        public void Save<T>(string path, T data) where T : class
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
                //TODO: Properly handle exceptions
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
            var jsonSerializer = new JsonSerializer();

            using (var streamReader = new StreamReader(path))
            {
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    var serializer = new JsonSerializer();

                    return serializer.Deserialize<T>(jsonReader);
                }
            }
        }


        /// <summary>
        /// Renames a file at the given <paramref name="path"/> to the given <paramref name="newName"/>.
        /// </summary>
        /// <param name="path">The path to the file to rename.</param>
        /// <param name="newName">The new name to give the file.  Any file extensions will be ignored.</param>
        public void Rename(string path, string newName)
        {
            if (!Path.HasExtension(path))
                throw new ArgumentException($"The path must have a file name with an extension.", nameof(path));

            newName = Path.HasExtension(newName) ? Path.GetFileNameWithoutExtension(newName) : newName;

            var pathSections = path.Split('\\');
            var oldFileName = Path.GetFileName(path);

            File.Move(path, $@"{pathSections.Join(oldFileName)}\{newName}{Path.GetExtension(oldFileName)}");
        }


        /// <summary>
        /// Returns a value indicating if the file at the given <paramref name="path"/> exists.
        /// </summary>
        /// <param name="path">The path to the file to check for.</param>
        /// <returns></returns>
        public bool Exists(string path)
        {
            return File.Exists(path);
        }


        /// <summary>
        /// Deletes the file at the given <paramref name="path"/>.
        /// </summary>
        /// <param name="path">The path to the file to delete.</param>
        public void Delete(string path)
        {
            File.Delete(path);
        }


        /// <summary>
        /// Copies the file at the given <paramref name="sourcePath"/> to the given <paramref name="destinationPath"/>.
        /// </summary>
        /// <param name="sourcePath">The source of the file to copy.</param>
        /// <param name="destinationPath">The destination of the file to copy.</param>
        public void Copy(string sourcePath, string destinationPath)
        {
            File.Copy(sourcePath, destinationPath);
        }
        #endregion
    }
}
