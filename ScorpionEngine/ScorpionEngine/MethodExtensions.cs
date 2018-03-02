using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScorpionEngine
{
    /// <summary>
    /// Provides extension methods for various things.
    /// </summary>
    public static class MethodExtensions
    {
        /// <summary>
        /// Gets the pixels of the texture.
        /// </summary>
        /// <returns></returns>
        public static Color[,] GetPixels(this Texture2D texture)
        {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            Color[,] colors2D = new Color[texture.Width, texture.Height];

            texture.GetData<Color>(colors1D);

            //Copy the data from the 1 dimensional array of color data to the 2 dimensional array of color data
            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    colors2D[x, y] = colors1D[x + y * texture.Width];
                }
            }

            return colors2D;
        }
    }
}
