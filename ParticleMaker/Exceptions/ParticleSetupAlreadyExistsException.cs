using System;

namespace ParticleMaker.Exceptions
{
    /// <summary>
    /// Occurs when a <see cref="ParticleSetup"/> already exists.
    /// </summary>
    public class ParticleSetupAlreadyExistsException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creats a new instance of <see cref="ParticleSetupAlreadyExistsException"/>.
        /// </summary>
        public ParticleSetupAlreadyExistsException() : base("The particle setup already exists.") { }


        /// <summary>
        /// Creats a new instance of <see cref="ParticleSetupAlreadyExistsException"/>.
        /// </summary>
        /// <param name="setupName">The name of the setup that already exists.</param>
        public ParticleSetupAlreadyExistsException(string setupName) : base($"The particle setup with the name '{setupName}' already exists.") { }
        #endregion
    }
}
