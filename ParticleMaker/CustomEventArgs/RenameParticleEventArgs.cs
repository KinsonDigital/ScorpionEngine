namespace ParticleMaker.CustomEventArgs
{
    /// <summary>
    /// Holds information about the event of renaming a particle.
    /// </summary>
    public class RenameParticleEventArgs
    {
        #region Constructors
        /// <summary>
        /// Create a new instance of <see cref="RenameParticleEventArgs"/>.
        /// </summary>
        /// <param name="particleName">The name of the particle being renamed.</param>
        /// <param name="particleFilePath">The path to the particle file.</param>
        public RenameParticleEventArgs(string particleName, string particleFilePath)
        {
            ParticleName = particleName;
            ParticleFilePath = particleFilePath;
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the name of the particle.
        /// </summary>
        public string ParticleName { get; set; }


        /// <summary>
        /// Gets or sets the path to the particle file.
        /// </summary>
        public string ParticleFilePath { get; set; }
        #endregion
    }
}
