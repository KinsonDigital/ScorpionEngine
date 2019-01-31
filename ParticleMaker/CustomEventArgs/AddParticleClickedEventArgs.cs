using System;
using ParticleMaker.UserControls;

namespace ParticleMaker.CustomEventArgs
{
    /// <summary>
    /// Occurs when the add particle button is clicked in the <see cref="ParticleList"/> control.
    /// </summary>
    public class AddParticleClickedEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="AddParticleClickedEventArgs"/>.
        /// </summary>
        /// <param name="particlePath">The path to the particle to add.</param>
        public AddParticleClickedEventArgs(string particlePath)
        {
            ParticleToAddPath = particlePath;
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the path to the particle to add.
        /// </summary>
        public string ParticleToAddPath { get; set; }
        #endregion
    }
}
