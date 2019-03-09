using ParticleMaker.Exceptions;
using ParticleMaker.Services;
using System.IO;
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
