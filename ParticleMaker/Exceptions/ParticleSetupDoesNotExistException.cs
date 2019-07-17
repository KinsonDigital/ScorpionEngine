using System;

namespace ParticleMaker.Exceptions
{
    /// <summary>
    /// Thrown when a <see cref="ParticleSetup"/> does not exist.
    /// </summary>
    public class ParticleSetupDoesNotExistException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creats a new instance of <see cref="ParticleSetupDoesNotExistException"/>.
        /// </summary>
        public ParticleSetupDoesNotExistException() : base("The particle setup does not exist.") { }


        /// <summary>
        /// Creats a new instance of <see cref="ParticleSetupDoesNotExistException"/>.
        /// </summary>
        /// <param name="setupName">The name of the setup that does not exist.</param>
        public ParticleSetupDoesNotExistException(string setupName) : base($"The particle setup with the name '{setupName}' does not exist.") { }
        #endregion
    }
}
