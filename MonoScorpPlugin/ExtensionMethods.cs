using KDScorpionCore;
using Microsoft.Xna.Framework;

namespace MonoScorpPlugin
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts the given <see cref="Microsoft.Xna.Framework.Vector2"/> to a <see cref="KDScorpionCore.Vector"/>.
        /// </summary>
        /// <param name="vector">The vector to convert.</param>
        /// <returns></returns>
        public static Vector ToVector(this Vector2 vector) => new Vector(vector.X, vector.Y);
    }
}
