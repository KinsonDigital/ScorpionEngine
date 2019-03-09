namespace ParticleMaker.CustomEventArgs
{
    /// <summary>
    /// Holds information about event when an add setup has been clicked.
    /// </summary>
    public class AddParticleEventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="AddParticleEventArgs"/>.
        /// </summary>
        /// <param name="particleFilePath">The file path of the particle to add.</param>
        public AddParticleEventArgs(string particleFilePath)
        {
            ParticleFilePath = particleFilePath;
        }


        #region Props
        /// <summary>
        /// Gets or sets the path to the particle to be added.
        /// </summary>
        public string ParticleFilePath { get; internal set; }
        #endregion
    }
}
