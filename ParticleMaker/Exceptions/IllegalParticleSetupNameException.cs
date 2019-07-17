using System;

namespace ParticleMaker.Exceptions
{
    /// <summary>
    /// Thrown when a particle setup name is illegal.
    /// </summary>
    public class IllegalParticleSetupNameException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="IllegalParticleSetupNameException"/>.
        /// </summary>
        public IllegalParticleSetupNameException() : base("Illegal particle setup name.  Cannot not use characters \\/:*?\"<>|") { }


        /// <summary>
        /// Creates a new instance of <see cref="IllegalParticleSetupNameException"/>.
        /// </summary>
        /// <param name="name">The name of the particle setup.</param>
        public IllegalParticleSetupNameException(string name) : base($"The particle setup '{name}'.  Cannot not use characters \\/:*?\"<>|") { }
        #endregion
    }
}
