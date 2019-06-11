using KDScorpionCore;
using KDScorpionCore.Graphics;
using System;
using System.Drawing;
using System.Linq;

namespace ScorpTestGame
{
    /// <summary>
    /// Provides simple utility methods.
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
        #endregion
    }
}
