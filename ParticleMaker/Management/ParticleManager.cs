﻿using ParticleMaker.Exceptions;
using ParticleMaker.Services;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ParticleMaker.Management
{
    /// <summary>
    /// Manages the particles for a project setup.
    /// </summary>
    public class ParticleManager
    {
        #region Fields
        private readonly IDirectoryService _directoryService;
        private readonly IFileService _fileService;
        private readonly string _rootProjectsPath;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ParticleManager"/>.
        /// </summary>
        /// <param name="directoryService">The directory service used to manage the project directories.</param>
        /// <param name="fileService">The file service used to manage particle files.</param>
        public ParticleManager(IDirectoryService directoryService, IFileService fileService)
        {
            _directoryService = directoryService;
            _fileService = fileService;

            _rootProjectsPath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Projects";
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Adds the new particle at the given <paramref name="particleSrcPath"/> in a setup with the given
        /// <paramref name="setupName"/> in a project with the given <paramref name="projectName"/>.
        /// </summary>
        /// <param name="projectName">The name of the project to add the particle to.</param>
        /// <param name="setupName">The name of the setup to add the particle to.</param>
        /// <param name="particleSrcPath">The file path to the particle to add/copy to the setup.</param>
        /// <param name="overwriteDestination">True if the particle should be overwritten in the setup directory.</param>
        public void AddParticle(string projectName, string setupName, string particleSrcPath, bool overwriteDestination = false)
        {
            if (ProjectExists(projectName))
            {
                if (SetupExists(projectName, setupName))
                {
                    var projectPath = $@"{_rootProjectsPath}\{projectName}";
                    var destPath = $@"{projectPath}\Setups\{setupName}\{Path.GetFileName(particleSrcPath)}";

                    _fileService.Copy(particleSrcPath, destPath, overwriteDestination);
                }
                else
                {
                    throw new ParticleSetupDoesNotExistException(setupName);
                }
            }
            else
            {
                throw new ProjectDoesNotExistException(projectName);
            }
        }


        /// <summary>
        /// Renames the <paramref name="currentParticleName"/> to the new given <paramref name="newParticleName"/>
        /// in a project that has the given <paramref name="projectName"/> and setup with the given <paramref name="setupName"/>.
        /// </summary>
        /// <param name="projectName">The name of the project owns the particle.</param>
        /// <param name="setupName">The name of the setup that owns the particle.</param>
        /// <param name="currentParticleName">The name of the particle to rename.</param>
        /// <param name="newParticleName">The new name to rename the particle to.</param>
        public void RenameParticle(string projectName, string setupName, string currentParticleName, string newParticleName)
        {
            if (ProjectExists(projectName))
            {
                if (SetupExists(projectName, setupName))
                {
                    var setupDirPath = $@"{_rootProjectsPath}\{projectName}\Setups\{setupName}";
                    var particleFilePath = $@"{setupDirPath}\{currentParticleName}.png";

                    _fileService.Rename(particleFilePath, newParticleName);
                }
                else
                {
                    throw new ParticleSetupDoesNotExistException(setupName);
                }
            }
            else
            {
                throw new ProjectDoesNotExistException(projectName);
            }
        }


        /// <summary>
        /// Deletes a particle with the given <paramref name="particleName"/> in a setup
        /// with the given <paramref name="setupName"/> in a project with the given <paramref name="projectName"/>.
        /// </summary>
        /// <param name="projectName">The name of the project that owns the particle.</param>
        /// <param name="setupName">The name of the setup that owns the particle.</param>
        /// <param name="particleName">The name of the particle.</param>
        public void DeleteParticle(string projectName, string setupName, string particleName)
        {
            if (ProjectExists(projectName))
            {
                if (SetupExists(projectName, setupName))
                {
                    var setupDirPath = $@"{_rootProjectsPath}\{projectName}\Setups\{setupName}";
                    var particleFilePath = $@"{setupDirPath}\{particleName}.png";

                    _fileService.Delete(particleFilePath);
                }
                else
                {
                    throw new ParticleSetupDoesNotExistException(setupName);
                }
            }
            else
            {
                throw new ProjectDoesNotExistException(projectName);
            }
        }


        /// <summary>
        /// Returns a list of the particle path paths for a setup that matches the given
        /// <paramref name="setupName"/> in a project that matches the given <paramref name="projectName"/>.
        /// </summary>
        /// <param name="projectName">The name of the project where the particles are located.</param>
        /// <param name="setupName">The name of the setup where the particles are located.</param>
        /// <returns></returns>
        public string[] GetParticlePaths(string projectName, string setupName)
        {
            if (ProjectExists(projectName))
            {
                var particlePath = $@"{_rootProjectsPath}\{projectName}\Setups\{setupName}";

                if (SetupExists(projectName, setupName))
                {
                    return (from f in _directoryService.GetFiles(particlePath)
                            where Path.HasExtension(f) && Path.GetExtension(f) == ".png"
                            select f).ToArray();
                }
                else
                {
                    throw new ParticleSetupDoesNotExistException(setupName);
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
        /// Returns a value indicating if a setup with the given <paramref name="setupName"/>
        /// exists in a project with the given <paramref name="projectName"/>.
        /// </summary>
        /// <param name="projectName">The name of the project that the setup exists in.</param>
        /// <param name="setupName">The name of the setup to check for.</param>
        /// <returns></returns>
        private bool SetupExists(string projectName, string setupName)
        {
            var setupDirectory = $@"{_rootProjectsPath}\{projectName}\Setups\{setupName}";
            var setupFilePath = $@"{setupDirectory}\{setupName}.json";


            return !string.IsNullOrEmpty(setupName) &&
                _directoryService.Exists(setupDirectory) &&
                _fileService.Exists(setupFilePath);
        }


        /// <summary>
        /// Returns a value indicating if a project with the given <paramref name="name"/> exists.
        /// </summary>
        /// <param name="name">The name of the project to check for.</param>
        /// <returns></returns>
        private bool ProjectExists(string name)
        {
            return !string.IsNullOrEmpty(name) && _directoryService.Exists($@"{_rootProjectsPath}\{name}");
        }
        #endregion
    }
}
