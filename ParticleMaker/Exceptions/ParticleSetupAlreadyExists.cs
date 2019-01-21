using System;
using ParticleMaker.Project;

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
        public ParticleSetupAlreadyExists() : base("A particle setup with that name already exists.")
        {
        }


        /// <summary>
        /// Creats a new instance of <see cref="ParticleSetupAlreadyExists"/>.
        /// </summary>
        /// <param name="setupName">The name of the setup that already exists.</param>
        public ParticleSetupAlreadyExists(string setupName) : base($"A particle setup with the name '{setupName}' already exists.")
        {
        }
        #endregion
    }
}
