using System;
using System.Collections.Generic;
using System.Linq;
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
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ProjectIOService"/>.
        /// </summary>
        /// <param name="directoryService">The service used to manage directories.</param>
        /// <param name="fileService">The service used to manage files.</param>
        public ProjectIOService(IDirectoryService directoryService, IFileService fileService)
        {

        }
        #endregion
    }
}
