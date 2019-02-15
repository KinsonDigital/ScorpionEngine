using System;

namespace ParticleMaker.Exceptions
{
    /// <summary>
    /// Occurs when a <see cref="ParticleSetup"/> already exists.
    /// </summary>
    public class ParticleSetupAlreadyExists : Exception
    {
        #region Constructors
        /// <summary>
        /// Creats a new instance of <see cref="ParticleSetupAlreadyExists"/>.
        /// </summary>
        public ParticleSetupAlreadyExists() : base("The particle setup already exists.")
        {
        }


        /// <summary>
        /// Creats a new instance of <see cref="ParticleSetupAlreadyExists"/>.
        /// </summary>
        /// <param name="setupName">The name of the setup that already exists.</param>
        public ParticleSetupAlreadyExists(string setupName) : base($"The particle setup with the name '{setupName}' already exists.")
        {
        }
        #endregion
    }
}
