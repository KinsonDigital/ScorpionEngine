using ParticleMaker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleMaker.Management
{
    /// <summary>
    /// Manages the particles for a project setup.
    /// </summary>
    public class ParticleManager
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ParticleManager"/>.
        /// </summary>
        /// <param name="directoryService">The directory service used to manage the project directories.</param>
        /// <param name="fileService">The file service used to manage particle files.</param>
        public ParticleManager(IDirectoryService directoryService, IFileService fileService)
        {

        }
        #endregion
    }
}
