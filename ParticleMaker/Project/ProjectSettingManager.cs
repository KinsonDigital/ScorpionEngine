using ParticleMaker.Exceptions;
using ParticleMaker.Services;
using System;
using System.IO;
using System.Reflection;

namespace ParticleMaker.Project
{
    /// <summary>
    /// Manages project related settings.
    /// </summary>
    public class ProjectSettingsManager
    {
        #region Fields
        private IDirectoryService _directoryService;
        private IFileService _fileService;
        private string _projectSettingsPath;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ProjectSettingsManager"/>
        /// </summary>
        public ProjectSettingsManager(IDirectoryService directoryService, IFileService fileService)
        {
            _directoryService = directoryService;
            _fileService = fileService;
            _projectSettingsPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Projects";
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Saves the project settings for the given project.
        /// </summary>
        /// <param name="projectName">The name of the project that the settings apply to.</param>
        /// <param name="settings">The project settings data to save.</param>
        public void Save(string projectName, ProjectSettings settings)
        {
            if (ProjectExists(projectName))
            {
                //Throw an exception if the project name is empty or null
                if (string.IsNullOrEmpty(projectName))
                {
                    throw new ArgumentException($"The {nameof(ProjectSettings)}.{nameof(ProjectSettings.ProjectName)} cannot be empty or null.", nameof(settings));
                }
                else if (settings.ProjectName.ContainsIllegalFileNameCharacters())//Illegal characters
                {
                    throw new IllegalFileNameCharactersException();
                }
                else
                {
                    _fileService.Create($@"{_projectSettingsPath}\{projectName}\{projectName}-project-settings.json", settings);
                }

                return;
            }

            throw new ProjectDoesNotExistException(projectName);
        }


        /// <summary>
        /// Loads the project settings for the given <paramref name="projectName"/>.
        /// </summary>
        /// <param name="projectName">The name of the project to load the settings from.</param>
        /// <returns></returns>
        public ProjectSettings Load(string projectName)
        {
            if (ProjectExists(projectName))
                return _fileService.Load<ProjectSettings>(projectName);

            throw new ProjectDoesNotExistException(projectName);
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Returns a value indicating if a project with the given <paramref name="name"/> exists.
        /// </summary>
        /// <param name="name">The name of the project to check for.</param>
        /// <returns></returns>
        private bool ProjectExists(string name)
        {
            return _directoryService.Exists($@"{_projectSettingsPath}\{name}");
        }
        #endregion
    }
}
