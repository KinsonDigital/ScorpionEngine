using System;

namespace ParticleMaker.Exceptions
{
    /// <summary>
    /// Occurs when a <see cref="ParticleSetup"/> do9es not exist.
    /// </summary>
    public class ParticleSetupDoesNotExist : Exception
    {
        #region Constructors
        /// <summary>
        /// Creats a new instance of <see cref="ParticleSetupDoesNotExist"/>.
        /// </summary>
        public ParticleSetupDoesNotExist() : base("The particle setup does not exist.")
        {
        }


        /// <summary>
        /// Creats a new instance of <see cref="ParticleSetupDoesNotExist"/>.
        /// </summary>
        /// <param name="setupName">The name of the setup that does not exist.</param>
        public ParticleSetupDoesNotExist(string setupName) : base($"The particle setup with the name '{setupName}' does not exist.")
        {
        }
        #endregion

    }
}
