using KDScorpionCore;
using System;

namespace ScorpionEngine.Utils
{
    /// <summary>
    /// Provides basic tools to make things easier internally in the engine.
    /// </summary>
    public static class Tools
    {
        private const float PI = 3.1415926535897931f;

        #region Public Methods
        /// <summary>
        /// Rotates the <paramref name="vector"/> around the <paramref name="rotateOrigin"/> at the given <paramref name="angle"/>.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="origin">The vector to rotate the <paramref name="vector"/> around.</param>
        /// <param name="angle">The angle in degrees to rotate <paramref name="vector"/>.  Value must be positive.</param>
        /// <returns></returns>
        public static Vector RotateAround(Vector vector, Vector origin, float angle, bool clockWise = true)
        {
            angle = clockWise ? angle.ToRadians() : angle.ToRadians() * -1;

            var cos = (float)Math.Cos(angle);
            var sin = (float)Math.Sin(angle);

            var dx = vector.X - origin.X;//The delta x
            var dy = vector.Y - origin.Y;//The delta y

            var tempX = dx * cos - dy * sin;
            var tempY = dx * sin + dy * cos;

            var x = tempX + origin.X;
            var y = tempY + origin.Y;


            return new Vector(x, y);
        }
        #endregion
    }
}
