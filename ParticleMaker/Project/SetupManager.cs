using KDParticleEngine;
using ParticleMaker.Exceptions;
using ParticleMaker.Services;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows.Media;

namespace ParticleMaker.Project
{
    /// <summary>
    /// Manages particle setup files for a project.
    /// </summary>
    public class SetupManager
    {
        #region Fields
        private readonly IDirectoryService _directoryService;
        private readonly IFileService _fileService;
        private readonly string _rootProjectsPath;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="SetupManager"/>.
        /// </summary>
        /// <param name="directoryService">The directory service used to manage directories.</param>
        /// <param name="fileService">The file service used to manage files.</param>
        /// <param name="projectName">The name of the project the setups belong to.</param>
        public SetupManager(IDirectoryService directoryService, IFileService fileService)
        {
            _directoryService = directoryService;
            _fileService = fileService;

            _rootProjectsPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Projects";
        }


        /// <summary>
        /// Gets a list of the setups in the project that matches the given <paramref name="projectName"/>.
        /// </summary>
        /// <param name="projectName">The name of the project.</param>
        /// <returns></returns>
        public string[] GetSetupNames(string projectName)
        {
            if (ProjectExists(projectName))
                return _directoryService.GetDirectories(projectName);

            throw new ProjectDoesNotExistException(projectName);
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Returns a list of directory paths to all of the setups in the given <paramref name="projectName"/>.
        /// </summary>
        /// <param name="projectName">The name of the project.</param>
        /// <returns></returns>
        public string[] GetSetupPaths(string projectName)
        {
            var projectPath = $@"{_rootProjectsPath}\{projectName}\Setups";


            return _directoryService.GetDirectories(projectPath);
        }


        /// <summary>
        /// Creates a new setup file using the given <paramref name="projectName"/>.
        /// </summary>
        /// <param name="setupName">The name to give the particle setup.</param>
        public void Create(string projectName, string setupName)
        {
            if (ProjectExists(projectName))
            {
                var projPath = $@"{_rootProjectsPath}\{projectName}";
                var setupDirectory = $@"{projPath}\Setups\{setupName}";
                var setupPath = $@"{setupDirectory}\{setupName}.json";

                //If the particle setup already exists, throw an exception
                if (_fileService.Exists(setupPath))
                {
                    throw new ParticleSetupAlreadyExists();
                }
                else
                {
                    //Check for any illegal characters in the setup name
                    if (ContainsIllegalCharacters(setupName))
                    {
                        throw new IllegalParticleSetupNameException(setupName);
                    }
                    else
                    {
                        //Create the setup directory first
                        _directoryService.Create(setupDirectory);

                        //Now create the setup JSON file itself
                        _fileService.Create(setupPath, new ParticleSetup());
                    }
                }
            }
            else
            {
                throw new ProjectDoesNotExistException(projectName);
            }
        }


        /// <summary>
        /// Loads the <see cref="ParticleSetup"/> data from a project that matches the given
        /// <paramref name="projectName"/> and a setup that matches the given <paramref name="setupName"/>.
        /// </summary>
        /// <param name="projectName">The name of the project that the setup is located in.</param>
        /// <param name="setupName">The name of the setup.</param>
        /// <returns></returns>
        public ParticleSetup Load(string projectName, string setupName)
        {
            if (ProjectExists(projectName))
            {
                var projPath = $@"{_rootProjectsPath}\{projectName}";
                var setupPath = $@"{projPath}\Setups\{setupName}\{setupName}.json";

                if (_fileService.Exists(setupPath))
                {
                    return _fileService.Load<ParticleSetup>(setupPath);
                }
                else
                {
                    throw new ParticleSetupDoesNotExist(setupName);
                }
            }

            throw new ProjectDoesNotExistException(projectName);
        }


        /// <summary>
        /// Saves the given particle <paramref name="setup"/> data in the project that matches the given <paramref name="projectName"/>
        /// to the setup that matches the given <paramref name="setupName"/>.
        /// </summary>
        /// <param name="projectName">The name of the project to save the setup to.</param>
        /// <param name="setupName">The name of the setup to save.</param>
        /// <param name="setup">The data to save to the setup.</param>
        public void Save(string projectName, string setupName, ParticleSetup setup)
        {
            var projPath = $@"{_rootProjectsPath}\{projectName}";
            var setupPath = $@"{projPath}\{setupName}.json";

            if (ProjectExists(projectName))
            {
                if (ContainsIllegalCharacters(setupName))
                {
                    throw new IllegalParticleSetupNameException(setupName);
                }
                else
                {
                    _fileService.Save(setupPath, setup);
                }
            }
            else
            {
                throw new ProjectDoesNotExistException(projectName);
            }
        }


        /// <summary>
        /// Renames the setup with the given <paramref name="setupName"/> to the given <paramref name="newName"/>.
        /// </summary>
        /// <param name="setupName">The current name of the setup to rename.</param>
        /// <param name="newName">The new name to name the setup.</param>
        public void Rename(string projectName, string setupName, string newName)
        {
            if (ProjectExists(projectName))
            {
                var projPath = $@"{_rootProjectsPath}\{projectName}";
                var setupPath = $@"{projPath}\{setupName}.json";

                //If the particle setup alread exists
                if (_fileService.Exists(setupPath))
                {
                    if (ContainsIllegalCharacters(newName))
                    {
                        throw new IllegalParticleSetupNameException(setupName);
                    }
                    else
                    {
                        _fileService.Rename(setupPath, "new-setup");
                    }
                }
                else
                {
                    throw new ParticleSetupDoesNotExist(setupName);
                }
            }
            else
            {
                throw new ProjectDoesNotExistException(projectName);
            }
        }


        /// <summary>
        /// Deletes the setup with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the setup to delete.</param>
        public void Delete(string projectName, string name)
        {
            var setupPath = $@"{_rootProjectsPath}\{projectName}\{name}.json";

            if (ProjectExists(projectName))
            {
                if (_fileService.Exists(setupPath))
                {
                    _fileService.Delete(setupPath);
                }
                else
                {
                    throw new ParticleSetupDoesNotExist(name);
                }
            }
            else
            {
                throw new ProjectDoesNotExistException(projectName);
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
            return !string.IsNullOrEmpty(name) && _directoryService.Exists($@"{_rootProjectsPath}\{name}");
        }


        /// <summary>
        /// Returns a value indicating if the given string <paramref name="value"/> contains any 
        /// illegal particle name characters.
        /// </summary>
        /// <param name="value">The string value to check.</param>
        /// <returns></returns>
        private bool ContainsIllegalCharacters(string value)
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
