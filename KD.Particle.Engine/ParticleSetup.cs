using System.Drawing;

namespace KDParticleEngine
{
    /// <summary>
    /// Holds the particle setup settings data for the <see cref="ParticleEngine"/> to consume.
    /// </summary>
    public class ParticleSetup
    {
        #region Props
        /// <summary>
        /// Gets or sets the minimum value of the red color component range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public byte RedMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum value of the red color component range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public byte RedMax { get; set; } = 255;

        /// <summary>
        /// Gets or sets the minimum value of the green color component range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public byte GreenMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum value of the green color component range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public byte GreenMax { get; set; } = 255;

        /// <summary>
        /// Gets or sets the minimum value of the blue color component range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public byte BlueMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum value of the blue color component range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public byte BlueMax { get; set; } = 255;

        /// <summary>
        /// Gets or sets the total number of particles that can be alive at once.
        /// </summary>
        public int TotalParticlesAliveAtOnce { get; set; }

        /// <summary>
        /// Gets or sets the minimum size of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float SizeMin { get; set; } = 0.5f;

        /// <summary>
        /// Gets or sets the maximum size of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float SizeMax { get; set; } = 1.5f;

        /// <summary>
        /// Gets or sets the minimum angle of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float AngleMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum angle of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float AngleMax { get; set; } = 360f;

        /// <summary>
        /// Gets or sets the minimum angular velocity of the range that a <see cref="Particle"/> be will randomly set to.
        /// </summary>
        public float AngularVelocityMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum angular velocity of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float AngularVelocityMax { get; set; } = 1.0f;

        /// <summary>
        /// Gets or sets the minimum X velocity of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float VelocityXMin { get; set; } = -1f;

        /// <summary>
        /// Gets or sets the maximum X velocity of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float VelocityXMax { get; set; } = 1f;

        /// <summary>
        /// Gets or sets the minimum X velocity of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float VelocityYMin { get; set; } = -1f;

        /// <summary>
        /// Gets or sets the maximum Y velocity of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public float VelocityYMax { get; set; } = 1f;

        /// <summary>
        /// Gets or sets the minimum life time of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int LifeTimeMin { get; set; } = 250;

        /// <summary>
        /// Gets or sets the maximum life time of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int LifeTimeMax { get; set; } = 1000;

        /// <summary>
        /// Gets or sets the minimum spawn rate of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int SpawnRateMin { get; set; } = 250;

        /// <summary>
        /// Gets or sets the maximum spawn rate of the range that a <see cref="Particle"/> will be randomly set to.
        /// </summary>
        public int SpawnRateMax { get; set; } = 1000;

        /// <summary>
        /// Gets or sets a value indicating if the colors will be randomly chosen from a list.
        /// </summary>
        public bool UseColorsFromList { get; set; } = true;

        /// <summary>
        /// Gets or sets the list of colors to randomly choose from.
        /// </summary>
        public Color[] Colors { get; set; } = new Color[]
        {
            Color.FromArgb(255, 255, 0, 0 ),
            Color.FromArgb(255, 0, 255, 0 ),
            Color.FromArgb(255, 0, 0, 255 )
        };
        #endregion
    }
}
