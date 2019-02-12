namespace KDParticleEngine
{
    /// <summary>
    /// Represents the color of a <see cref="Particle"/>.
    /// </summary>
    public class ParticleColor
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ParticleColor"/>.
        /// </summary>
        /// <param name="alpha">The alpa component of the <see cref="ParticleColor"/></param>
        /// <param name="red">The red component of the <see cref="ParticleColor"/></param>
        /// <param name="green">The green component of the <see cref="ParticleColor"/></param>
        /// <param name="blue">The blue component of the <see cref="ParticleColor"/></param>
        public ParticleColor(byte alpha = 0, byte red = 0, byte green = 0, byte blue= 0)
        {
            Alpha = alpha;
            Red = red;
            Green = green;
            Blue = blue;
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the alpha component of the color.
        /// </summary>
        public byte Alpha { get; set; }

        /// <summary>
        /// Gets or sets the red component of the color.
        /// </summary>
        public byte Red { get; set; }

        /// <summary>
        /// Gets or sets the green component of the color.
        /// </summary>
        public byte Green { get; set; }

        /// <summary>
        /// Gets or sets the blue component of the color.
        /// </summary>
        public byte Blue { get; set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Returns a value indicating if this object is equal to the given <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ParticleColor comparee))
                return false;


            return Alpha == comparee.Alpha &&
                   Red == comparee.Red &&
                   Green == comparee.Green &&
                   Blue == comparee.Blue;
        }


        /// <summary>
        /// Returns the hash code of this object that makes this object unique.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hashCode = -1096401256;

            hashCode = hashCode * -1521134295 + Alpha.GetHashCode();
            hashCode = hashCode * -1521134295 + Red.GetHashCode();
            hashCode = hashCode * -1521134295 + Green.GetHashCode();
            hashCode = hashCode * -1521134295 + Blue.GetHashCode();


            return hashCode;
        }
        #endregion
    }
}
