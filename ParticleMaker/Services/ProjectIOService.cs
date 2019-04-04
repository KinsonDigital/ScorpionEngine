using System.IO;
using System.Reflection;

namespace ParticleMaker.Services
{
    /// <summary>
    /// Provides various project related services.
    /// </summary>
    public class ProjectIOService
    {
        #region Fields
        private readonly IDirectoryService _directoryService;
        private readonly IFileService _fileService;
        private static string _projectsPath;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ProjectIOService"/>.
        /// </summary>
        /// <param name="directoryService">The service used to manage directories.</param>
        /// <param name="fileService">The service used to manage files.</param>
        public ProjectIOService(IDirectoryService directoryService, IFileService fileService)
        {
            _directoryService = directoryService;
            _fileService = fileService;

            _projectsPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Projects";
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Checks to make sure that the root projects folder already exists.
        /// If not, creates the root folder.
        /// </summary>
        public void CheckRootProjectsFolder()
        {
            if (_directoryService.Exists(_projectsPath))
                return;

            _directoryService.Create(_projectsPath);
        }


        /// <summary>
        /// Checks to make sure that the root setups folder already exists for the given project.
        /// If not, creates the setups folder.
        /// </summary>
        /// <param name="projectName">The name of the project.</param>
        public void CheckRootSetupsFolder(string projectName)
        {
            var setupsPath = $@"{_projectsPath}\{projectName}\Setups";

            if (_directoryService.Exists(setupsPath))
                return;

            _directoryService.Create(setupsPath);
        }


        /// <summary>
        /// Returns a value indicating if a project with the given <paramref name="name"/> exists.
        /// </summary>
        /// <param name="name">The name of the project.</param>
        /// <returns></returns>
        public bool ProjectExists(string name)
        {
            return !string.IsNullOrEmpty(name) && _directoryService.Exists($@"{_projectsPath}\{name}");
        }


        /// <summary>
        /// Returns a value indicating if a setup with the given <paramref name="setupName"/>
        /// exists in a project with the given <paramref name="projectName"/>.
        /// </summary>
        /// <param name="projectName">The name of the project.</param>
        /// <param name="setupName">The name of the setup.</param>
        /// <returns></returns>
        public bool SetupExists(string projectName, string setupName)
        {
            var setupDirectory = $@"{_projectsPath}\{projectName}\Setups\{setupName}";
            var setupFilePath = $@"{setupDirectory}\{setupName}.json";


            return !string.IsNullOrEmpty(setupName) &&
                _directoryService.Exists(setupDirectory) &&
                _fileService.Exists(setupFilePath);
        }


        /// <summary>
        /// Returns a value indicating if the given string <paramref name="value"/> contains any 
        /// illegal particle name characters.
        /// </summary>
        /// <param name="value">The string value to check.</param>
        /// <returns></returns>
        public bool ContainsIllegalCharacters(string value)
        {
            var characters = Path.GetInvalidFileNameChars();

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
