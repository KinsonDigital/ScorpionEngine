namespace ParticleMaker.CustomEventArgs
{
    /// <summary>
    /// Holds information about the event of renaming a particle.
    /// </summary>
    public class DeleteParticleEventArgs
    {
        #region Constructors
        /// <summary>
        /// Create a new instance of <see cref="DeleteParticleEventArgs"/>.
        /// </summary>
        /// <param name="particleName">The name of the particle being deleted.</param>
        /// <param name="particleFilePath">The path to the particle file.</param>
        public DeleteParticleEventArgs(string particleName, string particleFilePath)
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
