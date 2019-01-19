using System.IO;
using System.Reflection;
using ParticleMaker.Exceptions;

namespace ParticleMaker.Services
{
    /// <summary>
    /// Manages projects.
    /// </summary>
    public class ProjectService : IProjectService
    {
        #region Fields
        private readonly string _projectsPath;
        private IDirectoryService _directoryService;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ProjectService"/>.
        /// </summary>
        /// <param name="directoryService">The directory service to manage the project directories</param>
        public ProjectService(IDirectoryService directoryService)
        {
            _projectsPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Projects";

            _directoryService = directoryService;
        }

        /// <summary>
        /// Gets the list of projects.
        /// </summary>
        public string[] Projects => _directoryService.GetDirectories(_projectsPath);
        #endregion


        #region Public Methods
        /// <summary>
        /// Creates a new project using the given name.
        /// </summary>
        /// <param name="name">The name of the project to create.</param>
        public void Create(string name)
        {
            var newDirectory = $@"{_projectsPath}\{name}";

            CheckRootProjectsFolder();

            if (_directoryService.Exists(newDirectory))
                throw new ProjectAlreadyExistsException(name);
            else
                _directoryService.Create(newDirectory);
        }


        /// <summary>
        /// Deletes the project that matches the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the project to delete.</param>
        public void Delete(string name)
        {
            var projectDir = $@"{_projectsPath}\{name}";

            if (_directoryService.Exists(projectDir))
            {
                _directoryService.Delete(projectDir);
            }
            else
            {
                throw new ProjectDoesNotExistExistException(name);
            }
        }


        /// <summary>
        /// Renames the project with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="newName">The new name to give the project.</param>
        public void Rename(string name, string newName)
        {
            var oldProjectDir = $@"{_projectsPath}\{name}";
            var newProjecDir = $@"{_projectsPath}\{newName}";

            //If the project name is illegal, throw an exception
            if (string.IsNullOrEmpty(newName) || ContainsIllegalCharacters(newName))
                throw new IllegalProjectNameException(newName);

            if (_directoryService.Exists(oldProjectDir))
            {
                _directoryService.Rename(oldProjectDir, newProjecDir);
            }
            else
            {
                throw new ProjectDoesNotExistExistException(name);
            }
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Checks to make sure that the root projects folder already exists.
        /// If not, creates the root folder.
        /// </summary>
        private void CheckRootProjectsFolder()
        {
            if (_directoryService.Exists(_projectsPath))
                return;

            _directoryService.Create(_projectsPath);
        }


        /// <summary>
        /// Returns a value indicating if the given string <paramref name="value"/> contains any 
        /// illegal project name characters.
        /// </summary>
        /// <param name="value">The string value to check.</param>
        /// <returns></returns>
        private bool ContainsIllegalCharacters(string value)
        {
            var characters = new[]
            {
                '\\',
                '/',
                ':',
                '*',
                '?',
                '"',
                '<',
                '>',
                '|'
            };

            foreach (var c in characters)
            {
                if (value.Contains(c.ToString()))
                    return true;
            }


            return false;
        }
        #endregion
    }
}
