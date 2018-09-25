using System;
using SysColor = System.Drawing.Color;
using MonoColor = Microsoft.Xna.Framework.Color;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ScorpionCore;

namespace MonoScorpPlugin
{
    /// <summary>
    /// Provides basic tools to make things easier internally in the engine.
    /// </summary>
    public static class Tools
    {
        #region Public Methods
        /// <summary>
        /// Converts the given Vector2 to a standard Vector.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns></returns>
        public static Vector ToVector(Vector2 value)
        {
            return new Vector(value.X, value.Y);
        }


        /// <summary>
        /// Converts the given Vector to a Vector2.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns></returns>
        public static Vector2 ToVector2(Vector value)
        {
            return new Vector2(value.X, value.Y);
        }


        /// <summary>
        /// Converts the given Vector array into a Vector2 array.
        /// </summary>
        /// <param name="value">The vector array to convert.</param>
        /// <returns></returns>
        public static Vector2[] ToVector2(Vector[] value)
        {
            var returnValue = new Vector2[value.Length];

            //Save each value of the vector array to the vector2 array
            for (var i = 0; i < value.Length; i++)
            {
                //Convert the current element into a vector2 and add it to the vector2 array
                returnValue[i] = ToVector2(value[i]);
            }

            return returnValue;
        }


        /// <summary>
        /// Converts the given Vector2 array into a Vector array.
        /// </summary>
        /// <param name="value">The vector2 array to convert.</param>
        /// <returns></returns>
        public static Vector[] ToVector2(Vector2[] value)
        {
            var returnValue = new Vector[value.Length];

            //Save each value of the vector array to the vector2 array
            for (var i = 0; i < value.Length; i++)
            {
                //Convert the current element into a vector and add it to the vector array
                returnValue[i] = ToVector(value[i]);
            }

            return returnValue;
        }


        /// <summary>
        /// Converts the given Keys array into an InputKeys array.
        /// </summary>
        /// <param name="keys">The keys to convert.</param>
        /// <returns></returns>
        public static InputKeys[] ToInputKeys(Keys[] keys)
        {
            var returnArray = new InputKeys[keys.Length];

            //Convert each button into a InputKey
            for (var i = 0; i < keys.Length; i++)
            {
                returnArray[i] = (InputKeys) keys[i];
            }

            return returnArray;
        }


        /// <summary>
        /// Converts the given verticies to new verticies that takes a centered origin into account.
        /// </summary>
        /// <param name="verts">The verticies to convert.</param>
        /// <returns></returns>
        public static Vector[] ConvertForOrigin(Vector[] verts)
        {
            var returnValue = new Vector[verts.Length];
            var center = new Vector(17.5f, 19f);

            for (var i = 0; i < verts.Length; i++)
            {
                var isLeftOf = verts[i].X < center.X;
                var isAbove = verts[i].Y < center.Y;

                var delta = Math.Abs(verts[i].X - center.X);
                returnValue[i].X = isLeftOf ? delta * -1 : delta;

                delta = Math.Abs(verts[i].Y - center.Y);
                returnValue[i].Y = isAbove ? delta * -1 : delta;
            }

            return returnValue;
        }


        /// <summary>
        /// Gets the minimum value of the given enumeration.
        /// </summary>
        /// <param name="enumType">The enum to process.</param>
        /// <returns></returns>
        public static int GetEnumMin(Type enumType)
        {
            var values = Enum.GetValues(enumType);

            //Get the smallest item out of all of the values
            return values.Cast<int>().Concat(new[] {int.MaxValue}).Min();
        }


        /// <summary>
        /// Gets the maximum value of the given enumeration.
        /// </summary>
        /// <param name="enumType">The enum to process.</param>
        /// <returns></returns>
        public static int GetEnumMax(Type enumType)
        {
            var values = Enum.GetValues(enumType);

            //Get the largest item out of all of the values
            return values.Cast<int>().Concat(new[] {0}).Max();
        }

        public static SysColor ToSysColor(this MonoColor color)
        {
            return SysColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static MonoColor ToMonoColor(this SysColor color)
        {
            return new MonoColor(color.R, color.G, color.B, color.A);
        }
        #endregion
    }
}