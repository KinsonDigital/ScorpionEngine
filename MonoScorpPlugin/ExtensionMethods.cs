using KDScorpionCore;
using KDScorpionCore.Graphics;
using Microsoft.Xna.Framework;

namespace MonoScorpPlugin
{
    /// <summary>
    /// Provides extensions to various things to help make better code.
    /// </summary>
    public static class ExtensionMethods
    {
        #region Public Methods
        /// <summary>
        /// Converts the given <see cref="Vector2"/> to a <see cref="Vector"/>.
        /// </summary>
        /// <param name="vector">The vector to convert.</param>
        /// <returns></returns>
        public static Vector ToVector(this Vector2 vector) => new Vector(vector.X, vector.Y);


        /// <summary>
        /// Converts the given <see cref="Rect"/> to a <see cref="Rectangle"/>.
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
        public static Color ToXNAColor(this GameColor color) =>
            new Color(color.Red, color.Green, color.Blue, color.Alpha);
        #endregion
    }
}
