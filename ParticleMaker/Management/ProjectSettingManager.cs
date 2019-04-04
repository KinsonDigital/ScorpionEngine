using ParticleMaker.Exceptions;
using ParticleMaker.Services;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ParticleMaker.Management
{
    /// <summary>
    /// Manages project related settings.
    /// </summary>
    public class ProjectSettingsManager
    {
        #region Fields
        private readonly ProjectIOService _projIOService;
        private IDirectoryService _directoryService;
        private IFileService _fileService;
        private string _projectSettingsPath;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ProjectSettingsManager"/>
        /// </summary>
        /// <param name="directoryService">The directory service used to manage the project directories.</param>
        /// <param name="fileService">The file service used to manage project setting files.</param>
        public ProjectSettingsManager(ProjectIOService projIOService, IDirectoryService directoryService, IFileService fileService)
        {
            _projIOService = projIOService;
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
                if (settings.ProjectName.ContainsIllegalFileNameCharacters())//Illegal characters
                {
                    throw new IllegalFileNameCharactersException();
                }
                else
                {
                    _fileService.Create($@"{_projectSettingsPath}\{projectName}\{projectName}-project-settings.json", settings);
                }

                return;
            }
            else
            {
                throw new ProjectDoesNotExistException(projectName);
            }
        }


        /// <summary>
        /// Loads the project settings for the given <paramref name="projectName"/>.
        /// </summary>
        /// <param name="projectName">The name of the project to load the settings from.</param>
        /// <returns></returns>
        public ProjectSettings Load(string projectName)
        {
            if (ProjectExists(projectName))
            {
                var filePath = $@"{_projectSettingsPath}\{projectName}\{projectName}-project-settings.json";

                var projSettings = _fileService.Load<ProjectSettings>(filePath);

                if (projSettings != null && projSettings.SetupDeploySettings == null)
                    projSettings.SetupDeploySettings = new DeploymentSetting[0];

                return projSettings;
            }

            throw new ProjectDoesNotExistException(projectName);
        }


        /// <summary>
        /// Renames the project settings file from the given <paramref name="projectName"/> to
        /// the given <paramref name="newProjectName"/>.
        /// </summary>
        /// <param name="projectName">The current name of the project settings file.</param>
        /// <param name="newProjectName">The new project name.</param>
        public void Rename(string projectName, string newProjectName)
        {
            if (ProjectExists(projectName))
            {
                var oldFilePath = $@"{_projectSettingsPath}\{projectName}\{projectName}-project-settings.json";

                _fileService.Rename(oldFilePath, $"{newProjectName}-project-settings.json");
            }
            else
            {
                throw new ProjectDoesNotExistException(projectName);
            }
        }


        /// <summary>
        /// Renames the setup name for a setup that matches the given <paramref name="currentSetupName"/>
        /// for a project that matches the given <paramref name="projectName"/>.
        /// </summary>
        /// <param name="projectName">The name of the project that contains the <see cref="ProjectSettings"/>.</param>
        /// <param name="currentSetupName">The name of the setup to change.</param>
        /// <param name="newSetupName">The new name to change the <paramref name="currentSetupName"/> to.</param>
        public void RenameDeploymentSetupName(string projectName, string currentSetupName, string newSetupName)
        {
            if (ProjectExists(projectName))
            {
                var projectDirPath = $@"{_projectSettingsPath}\{projectName}\{projectName}-project-settings.json";

                var projSettings = _fileService.Load<ProjectSettings>(projectDirPath);

                var deploySetting = (from s in projSettings.SetupDeploySettings
                                     where s.SetupName == currentSetupName
                                     select s).FirstOrDefault();

                if (deploySetting == null)
                    throw new Exception($"No deploy setting data for project '{projectName}' and setup '{currentSetupName}'.");

                var originalItemIndex = projSettings.SetupDeploySettings.ToList().IndexOf(deploySetting);

                //Update the name of the setup to the new setup name
                deploySetting.SetupName = newSetupName;

                //Overwrite the old deploy setting with the new updated setting
                projSettings.SetupDeploySettings[originalItemIndex] = deploySetting;

                _fileService.Save(projectDirPath, projSettings);
            }
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
            return !string.IsNullOrEmpty(name) && _directoryService.Exists($@"{_projectSettingsPath}\{name}");
        }
        #endregion
    }
}
