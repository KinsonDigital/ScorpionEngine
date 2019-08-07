using KDParticleEngine;
using KDScorpionCore;
using KDScorpionCore.Graphics;
using System;
using System.Drawing;
using System.Linq;

namespace ScorpTestGame
{
    /// <summary>
    /// Provides extensions to various things to help make better code.
    /// </summary>
    public static class ExtensionMethods
    {
        #region Methods
        /// <summary>
        /// Converts the given <see cref="GameColor"/> to the <see cref="System.Drawing.Color"/>.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns></returns>
        public static Color ToNETColor(this GameColor color) => Color.FromArgb(color.Alpha, color.Red, color.Green, color.Blue);


        /// <summary>
        /// Converts the given <paramref name="color"/> of type <see cref="Color"/> to the type <see cref="GameColor"/>.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns></returns>
        public static GameColor ToGameColor(this Color color) => new GameColor(color.A, color.R, color.G, color.B);


        /// <summary>
        /// Converts the given <paramref name="colors"/> of type <see cref="GameColor"/> to an array of <see cref="System.Drawing.Color"/>s.
        /// </summary>
        /// <param name="colors">The colors to convert.</param>
        /// <returns></returns>
        public static Color[] ToNETColors(this GameColor[] colors) => (from c in colors select c.ToNETColor()).ToArray();


        /// <summary>
        /// Converts the given <see cref="TimeSpan"/> to the type <see cref="EngineTime"/>.
        /// </summary>
        /// <param name="time">The time to convert.</param>
        /// <returns></returns>
        public static EngineTime ToEngineTime(this TimeSpan time) => new EngineTime() { ElapsedEngineTime = time, TotalEngineTime = new TimeSpan() };


        /// <summary>
        /// Converts the given <see cref="TimeSpan"/> to the type <see cref="EngineTime"/>.
        /// </summary>
        /// <param name="time">The time to convert.</param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(this EngineTime time) => time.ElapsedEngineTime;


        /// <summary>
        /// Converts the given <paramref name="vector"/> of type <see cref="Vector"/> to the type <see cref="PointF"/>.
        /// </summary>
        /// <param name="vector">The vector to convert.</param>
        /// <returns></returns>
        public static PointF ToPointF(this Vector vector) => new PointF(vector.X, vector.Y);


        /// <summary>
        /// Converts the given <paramref name="point"/> of type <see cref="PointF"/> to type <see cref="Vector"/>.
        /// </summary>
        /// <param name="point">The point to convert.</param>
        /// <returns></returns>
        public static Vector ToVector(this PointF point) => new Vector(point.X, point.Y);


        /// <summary>
        /// Renders a particle to the screen.
        /// </summary>
        /// <param name="renderer">The renderer to use to render the particle.</param>
        /// <param name="particle">The particle to render.</param>
        public static void RenderParticle(this Renderer renderer, Particle<Texture> particle)
        {
            renderer.Render(particle.Texture, particle.Position.X, particle.Position.Y, particle.Angle, particle.Size, particle.TintColor.ToGameColor());
        }
        #endregion
    }
}
