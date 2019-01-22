﻿using System.IO;
using System.Reflection;
using ParticleMaker.Exceptions;
using ParticleMaker.Services;

namespace ParticleMaker.Project
{
    /// <summary>
    /// Manages projects.
    /// </summary>
    public class ProjectManager
    {
        #region Fields
        private ProjectSettingsManager _settingsService;
        private IDirectoryService _directoryService;
        private static string _projectsPath;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ProjectManager"/>.
        /// </summary>
        /// <param name="directoryService">The directory service to manage the project directories</param>
        public ProjectManager(ProjectSettingsManager settingsService, IDirectoryService directoryService)
        {
            _settingsService = settingsService;
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
            {
                throw new ProjectAlreadyExistsException(name);
            }
            else
            {
                //If the project name is illegal, throw an exception
                if (string.IsNullOrEmpty(name) || ContainsIllegalCharacters(name))
                    throw new IllegalProjectNameException(name);

                _directoryService.Create(newDirectory);

                var newSettings = new ProjectSettings();

                _settingsService.Save(name, newSettings);
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
                throw new ProjectDoesNotExistException(name);
            }
        }


        /// <summary>
        /// Returns a value indicating if a project with the given <paramref name="name"/> exists.
        /// </summary>
        /// <param name="name">The name of the project to check for.</param>
        /// <returns></returns>
        public bool Exists(string name)
        {
            return _directoryService.Exists($@"{_projectsPath}\{name}");
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
            var characters = Path.GetInvalidPathChars();
            
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