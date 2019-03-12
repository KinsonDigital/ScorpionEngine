using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace ParticleMaker.Services
{
    /// <summary>
    /// Manages directories using such operations such as create and exists.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DirectoryService : IDirectoryService
    {
        #region Public Methods
        /// <summary>
        /// Creates a new project using the given <paramref name="path"/>.
        /// NOTE: Projects must have a unique name in the projects root directory and names must follow
        /// the OS directory naming rules.
        /// </summary>
        /// <param name="path">The name of the project.</param>
        public void Create(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("The argument cannot be null or empty.", nameof(path));

            Directory.CreateDirectory(path);
        }


        /// <summary>
        /// Returns a value indicating if the given <paramref name="path"/> exists.
        /// </summary>
        /// <param name="path">The directory path to check for.</param>
        /// <returns></returns>
        public bool Exists(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("The argument cannot be null or empty.", nameof(path));
            

            return Directory.Exists(path);
        }


        /// <summary>
        /// Deletes the given directory path and all of its contents.
        /// </summary>
        /// <param name="path">The directory to delete.</param>
        /// <returns></returns>
        public void Delete(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("The argument cannot be null or empty.", nameof(path));

            Directory.Delete(path, true);
        }


        /// <summary>
        /// Renames the given directory at the <paramref name="path"/> to the new given directory namne.
        /// </summary>
        /// <param name="path">The directory to rename</param>
        /// <param name="newName">The new name to give the directory.</param>
        public void Rename(string path, string newName)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("The argument cannot be null or empty.", nameof(path));

            if (string.IsNullOrEmpty(newName))
                throw new ArgumentException("The argument cannot be null or empty.", nameof(path));

            if (path.Split('\\').Length <= 0)
                throw new ArgumentException("The argument must be a valid directory path.", nameof(path));

            var dirs = path.Split('\\');

            dirs[dirs.Length - 1] = newName;

            Directory.Move(path, dirs.Join());
        }


        /// <summary>
        /// Returns a list of directories at the given path.
        /// </summary>
        /// <param name="path">The directory path of where to get the list of directories from.</param>
        /// <returns></returns>
        public string[] GetDirectories(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("The argument cannot be null or empty.", nameof(path));


            return Directory.GetDirectories(path);
        }


        /// <summary>
        /// Returns the names of files (including their paths) in the given directory.
        /// </summary>
        /// <param name="path">The directory to check for files in.</param>
        /// <returns></returns>
        public string[] GetFiles(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("The argument cannot be null or empty.", nameof(path));


            return Directory.GetFiles(path);
        }
        #endregion
    }
}
