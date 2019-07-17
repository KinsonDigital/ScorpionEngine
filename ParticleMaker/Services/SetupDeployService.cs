using ParticleMaker.Exceptions;
using System.IO;
using System.Reflection;

namespace ParticleMaker.Services
{
    /// <summary>
    /// Provides ability to deploy setups to destination locations.
    /// </summary>
    public class SetupDeployService
    {
        #region Private Fields
        private readonly IDirectoryService _directoryService;
        private readonly IFileService _fileService;
        private readonly string _rootProjectsPath;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="SetupDeployService"/>.
        /// </summary>
        /// <param name="directoryService">The directory service used to manage directories.</param>
        /// <param name="fileService">The file service used to manage files.</param>
        /// <param name="projectName">The name of the project to point to for setup deployment.</param>
        public SetupDeployService(IDirectoryService directoryService, IFileService fileService)
        {
            _directoryService = directoryService;
            _fileService = fileService;

            _rootProjectsPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Projects";
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Deploys the setup with the given <paramref name="setupName"/> to the given <paramref name="destinationPath"/>.
        /// </summary>
        /// <param name="setupName">The name of the setup to deploy.</param>
        /// <param name="destinationPath">The destination folder path of where to deploy the setup.</param>
        public void Deploy(string projectName, string setupName, string destinationPath)
        {
            var projPath = $@"{_rootProjectsPath}\{projectName}";
            var setupPath = $@"{projPath}\Setups\{setupName}\{setupName}.json";

            if (ProjectExists(projectName))
            {
                if (!_directoryService.Exists(destinationPath))
                    throw new DirectoryNotFoundException($"The destination path '{Path.GetDirectoryName(destinationPath)}' does not exist.");

                CheckRootSetupsFolder(projectName);

                _fileService.Copy(setupPath, $@"{destinationPath}\{setupName}.json", true);
            }
            else
            {
                throw new ProjectDoesNotExistException();
            }
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Returns a value indicating if a project with the given <paramref name="name"/> exists.
        /// </summary>
        /// <param name="name">The name of the project to check for.</param>
        /// <returns></returns>
        private bool ProjectExists(string name) => !string.IsNullOrEmpty(name) && _directoryService.Exists($@"{_rootProjectsPath}\{name}");


        /// <summary>
        /// Checks to make sure that the root setups folder already exists for the project that
        /// matches the given <paramref name="projectName"/>.  If not, creates the setups folder.
        /// </summary>
        /// <param name="projectName">The name of the project.</param>
        private void CheckRootSetupsFolder(string projectName)
        {
            var setupsPath = $@"{_rootProjectsPath}\{projectName}\Setups";

            if (_directoryService.Exists(setupsPath))
                return;

            _directoryService.Create(setupsPath);
        }
        #endregion
    }
}
