namespace ParticleMaker.CustomEventArgs
{
    /// <summary>
    /// Holds information about event when a particle has been added.
    /// </summary>
    public class AddParticleEventArgs
    {
        #region Public Methods
        /// <summary>
        /// Creates a new instance of <see cref="AddParticleEventArgs"/>.
        /// </summary>
        /// <param name="particleFilePath">The file path of the particle to add.</param>
        public AddParticleEventArgs(string particleFilePath) => ParticleFilePath = particleFilePath;
        #endregion


        #region Props
        /// <summary>
        /// Gets the path to the particle to be added.
        /// </summary>
        public string ParticleFilePath { get; internal set; }
        #endregion
    }
}
