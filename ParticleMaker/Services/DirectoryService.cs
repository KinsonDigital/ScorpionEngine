using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

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
        /// Creates a new project using the given <paramref name="folder"/>.
        /// NOTE: Projects must have a unique name in the projects root folder and names must follow
        /// the OS directory naming rules.
        /// </summary>
        /// <param name="folder">The name of the project.</param>
        public void Create(string folder)
        {
            if (!Exists(folder))
                Directory.CreateDirectory(folder);
        }


        /// <summary>
        /// Returns a value indicating if the given folder <paramref name="folder"/> exists.
        /// </summary>
        /// <param name="folder">The folder path to check for.</param>
        /// <returns></returns>
        public bool Exists(string folder)
        {
            return Directory.Exists(folder);
        }


        /// <summary>
        /// Deletes the given folder path and all of its contents.
        /// </summary>
        /// <param name="folder">The folder to delete.</param>
        /// <returns></returns>
        public void Delete(string folder)
        {
            Directory.Delete(folder, true);
        }


        /// <summary>
        /// Renames the given <paramref name="folder"/> to the given 
        /// </summary>
        /// <param name="folder">The folder to rename</param>
        /// <param name="newName">The new name to give the folder.</param>
        public void Rename(string folder, string newName)
        {
            var folders = folder.Split('\\');
            folders[folders.Length - 1] = newName;

            var newFolderPath = new StringBuilder();

            for (int i = 0; i < folders.Length; i++)
            {
                bool useForwardSlash = folders[i] != newName;

                newFolderPath.Append($@"{folders[i]}{(useForwardSlash ? @"\" : "")}");
            }

            Directory.Move(folder, newFolderPath.ToString());
        }
        #endregion
    }
}
