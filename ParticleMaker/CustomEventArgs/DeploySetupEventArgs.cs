using System;

namespace ParticleMaker.CustomEventArgs
{
    /// <summary>
    /// Holds the information of where to deploy a setup.
    /// </summary>
    public class DeploySetupEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="DeploySetupEventArgs"/>.
        /// </summary>
        /// <param name="deploymentPath">The destination path of where to deploy a setup.</param>
        public DeploySetupEventArgs(string deploymentPath) => DeploymentPath = deploymentPath;
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the destination of where to deploy a setup.
        /// </summary>
        public string DeploymentPath { get; private set; }
        #endregion
    }
}
