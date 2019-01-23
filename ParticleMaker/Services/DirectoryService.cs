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
            if (!Exists(path))
                Directory.CreateDirectory(path);
        }


        /// <summary>
        /// Returns a value indicating if the given <paramref name="path"/> exists.
        /// </summary>
        /// <param name="path">The directory path to check for.</param>
        /// <returns></returns>
        public bool Exists(string path)
        {
            return Directory.Exists(path);
        }


        /// <summary>
        /// Deletes the given directory path and all of its contents.
        /// </summary>
        /// <param name="path">The directory to delete.</param>
        /// <returns></returns>
        public void Delete(string path)
        {
            Directory.Delete(path, true);
        }


        /// <summary>
        /// Renames the given directory at the <paramref name="path"/> to the new given directory namne.
        /// </summary>
        /// <param name="path">The directory to rename</param>
        /// <param name="newName">The new name to give the directory.</param>
        public void Rename(string path, string newName)
        {
            var dirs = path.Split('\\');
            dirs[dirs.Length - 1] = newName;

            Directory.Move(path, dirs.Join());
        }


        /// <summary>
        /// Returns a list of directories at the given path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string[] GetDirectories(string path)
        {
            var dirs = Directory.GetDirectories(path);

            for (int i = 0; i < dirs.Length; i++)
            {
                dirs[i] = Path.GetDirectoryName(dirs[i]);
            }


            return dirs;
        }
        #endregion
    }
}
