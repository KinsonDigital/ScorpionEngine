using System.IO;
using System.Reflection;
using ParticleMaker.Exceptions;

namespace ParticleMaker.Services
{
    /// <summary>
    /// Manages projects.
    /// </summary>
    public class ProjectService
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
        #endregion
    }
}
