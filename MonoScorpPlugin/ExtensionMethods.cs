using KDScorpionCore;
using KDScorpionCore.Graphics;
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


        /// <summary>
        /// Converts the given <see cref="KDScorpionCore.Rect"/> to a <see cref="Microsoft.Xna.Framework.Rectangle"/>.
        /// </summary>
        /// <param name="rect">The rectangle to convert.</param>
        /// <returns></returns>
        public static Rectangle ToRectangle(this Rect rect) => new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);


        /// <summary>
        /// Converts the given <paramref name="color"/> of type <see cref="GameColor"/>
        /// to the type <see cref="Color"/>.
        /// </summary>
        /// <param name="color">The array of colors to convert.</param>
        /// <returns></returns>
        public static Color ToGameColor(this GameColor color) =>
            new Color(color.Red, color.Green, color.Blue, color.Alpha);
    }
}
