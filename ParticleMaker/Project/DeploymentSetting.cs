namespace ParticleMaker.Project
{
    /// <summary>
    /// Gets or sets the deployement setting of a particle setup.
    /// </summary>
    public class DeploymentSetting
    {
        #region Props
        /// <summary>
        /// Gets or sets the name of the setup to deploy.
        /// </summary>
        public string SetupName { get; set; }

        /// <summary>
        /// Gets or sets the name of the path of where to deploy the setup.
        /// </summary>
        public string DeployPath { get; set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Returns a value indicating if this object is eqaul to another <see cref="DeploymentSetting"/> object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var otherObj = (DeploymentSetting)obj;

            return SetupName == otherObj.SetupName &&
                   DeployPath == otherObj.DeployPath;
        }


        /// <summary>
        /// Returns the hash code for this this <see cref="DeploymentSetting"/>.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return SetupName.GetHashCode() + DeployPath.GetHashCode();
        }
        #endregion
    }
}