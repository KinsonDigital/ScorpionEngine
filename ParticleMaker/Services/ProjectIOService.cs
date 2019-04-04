using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParticleMaker.Services
{
    /// <summary>
    /// Provides various project related services such and project and
    /// setup checks to checking for illegal characters for file and directory management.
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
        /// Checks to make sure that the root setups folder already exists for a particular project.
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
        /// <param name="name">The name of the project to check for.</param>
        /// <returns></returns>
        public bool ProjectExists(string name)
        {
            return !string.IsNullOrEmpty(name) && _directoryService.Exists($@"{_projectsPath}\{name}");
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
