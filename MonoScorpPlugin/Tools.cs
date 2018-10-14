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
        /// <summary>
        /// Converts the given Keys array into an InputKeys array.
        /// </summary>
        /// <param name="keys">The keys to convert.</param>
        /// <returns></returns>
        public static int[] ToInputKeyCodes(Keys[] keys)
        {
            var returnArray = new int[keys.Length];

            //Convert each button into a InputKey
            for (var i = 0; i < keys.Length; i++)
            {
                returnArray[i] = (int)keys[i];
            }

            return returnArray;
        }
    }
}