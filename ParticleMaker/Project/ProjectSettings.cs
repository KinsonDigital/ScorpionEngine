namespace ParticleMaker.Project
{
    /// <summary>
    /// Holds various project related settings.
    /// </summary>
    public class ProjectSettings
    {
        #region Props
        /// <summary>
        /// Holds all of the deployement settings for all of a project's setups.
        /// </summary>
        public DeploymentSetting[] SetupDeploySettings { get; set; }

        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        public object ProjectName { get; internal set; }
        #endregion
    }
}
