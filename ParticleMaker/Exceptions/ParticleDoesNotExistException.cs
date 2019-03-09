using System;

namespace ParticleMaker.Exceptions
{
    /// <summary>
    /// Occurs when a particle does not exist.
    /// </summary>
    public class ParticleDoesNotExistException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ParticleDoesNotExistException"/>.
        /// </summary>
        public ParticleDoesNotExistException() : base("The particle does not exist.")
        {
        }


        /// <summary>
        /// Creates a new instance of <see cref="ParticleDoesNotExistException"/>.
        /// </summary>
        /// <param name="particleName">The name of the particle that does not exist.</param>
        /// <param name="particlePath">The path to the particle that does not exist.</param>
        public ParticleDoesNotExistException(string particleName, string particlePath) : base($"The particle '{particleName}' at the path '{particlePath}' does not exist.")
        {
        }
        #endregion
    }
}
