﻿using System.IO;
using System.Linq;
using System.Reflection;
using ParticleMaker.Exceptions;
using ParticleMaker.Services;

namespace ParticleMaker.Management
{
    /// <summary>
    /// Manages projects.
    /// </summary>
    public class ProjectManager
    {
        #region Fields
        private readonly ProjectSettingsManager _settingsManager;
        private readonly ProjectIOService _projIOService;
        private readonly IDirectoryService _directoryService;
        private readonly IFileService _fileService;
        private static string _projectsPath;
        private const string PROJ_FILE_EXTENSION = ".projs";
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ProjectManager"/>.
        /// </summary>
        /// <param name="settingsManager">The settings manager used to create project settings.</param>
        /// <param name="projIOService">The service used to manage common project management tasks.</param>
        /// <param name="directoryService">The directory service used to manage the project directories.</param>
        public ProjectManager(ProjectSettingsManager settingsManager, ProjectIOService projIOService, IDirectoryService directoryService, IFileService fileService)
        {
            _settingsManager = settingsManager;
            _projIOService = projIOService;
            _directoryService = directoryService;
            _fileService = fileService;

            _projectsPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Projects";

            _projIOService.CheckRootProjectsFolder();

            _directoryService = directoryService;
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets the list of projects.
        /// </summary>
        public string[] Projects
        {
            get
            {
                _projIOService.CheckRootProjectsFolder();


                return _directoryService.GetDirectories(_projectsPath)?.Where(d =>
                {
                    return !string.IsNullOrEmpty(d) && d.Split('\\').Length > 0;
                }).Select(d =>
                {
                    var sections = d.Split('\\');

                    return sections[^1];
                }).ToArray();
            }
        }

        /// <summary>
        /// Gets all of the directory paths to all of the project files.
        /// </summary>
        public string[] ProjectFilePaths
        {
            get
            {
                _projIOService.CheckRootProjectsFolder();


                return _directoryService.GetDirectories(_projectsPath).Where(path =>
                {
                    var dirSections = path == null ? new string[0] : path.Split('\\');
                    var dir = dirSections.Length >= 1 ? dirSections[^1] : string.Empty;

                    return !string.IsNullOrEmpty(dir) && _fileService.Exists($@"{path}\{dir}{PROJ_FILE_EXTENSION}");
                }).Select(d => d).ToArray();
            }
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

            if (_directoryService.Exists(newDirectory))
            {
                throw new ProjectAlreadyExistsException(name);
            }
            else
            {
                //If the project name is illegal, throw an exception
                if (string.IsNullOrEmpty(name) || name.ContainsIllegalFileNameCharacters())
                    throw new IllegalProjectNameException(name);

                _directoryService.Create(newDirectory);

                var newSettings = new ProjectSettings()
                {
                    ProjectName = name,
                };

                _settingsManager.Save(name, newSettings);
            }
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
                throw new ProjectDoesNotExistException(name);
            }
        }


        /// <summary>
        /// Renames the project with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the project to rename.</param>
        /// <param name="newName">The new name to give the project.</param>
        public void Rename(string name, string newName)
        {
            //If the project name is illegal, throw an exception
            if (string.IsNullOrEmpty(newName) || newName.ContainsIllegalFileNameCharacters())
                throw new IllegalProjectNameException(newName);

            var oldProjectDir = $@"{_projectsPath}\{name}";

            if (_directoryService.Exists(oldProjectDir))
            {
                _directoryService.Rename(oldProjectDir, newName);
            }
            else
            {
                throw new ProjectDoesNotExistException(name);
            }
        }


        /// <summary>
        /// Returns a value indicating if a project with the given <paramref name="name"/> exists.
        /// </summary>
        /// <param name="name">The name of the project to check for.</param>
        /// <returns></returns>
        public bool Exists(string name) => _directoryService.Exists($@"{_projectsPath}\{name}");
        #endregion
    }
}
