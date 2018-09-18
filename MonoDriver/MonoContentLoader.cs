using Microsoft.Xna.Framework.Graphics;
using ScorpionEngine.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoDriver
{
    public class MonoContentLoader : IContentLoader
    {
        /// <summary>
        /// Gets the path to the executable game.
        /// </summary>
        public static string GamePath { get; } = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

        /// <summary>
        /// Gets or sets the root directory for the game's content.
        /// </summary>
        public static string ContentRootDirectory { get; set; } = GamePath + "\\" + "Content";
        
        
        public ITexture LoadTexture(string textureName)
        {
            var fullPath = ContentRootDirectory + "\\" + Path.GetDirectoryName(textureName) + "Graphics\\" + Path.GetFileName(textureName) + ".png";

            var dir = Path.GetDirectoryName(textureName);

            MonoTexture newTexture;

            using (var fileStream = File.OpenRead(fullPath))
            {
                newTexture = new MonoTexture(Texture2D.FromStream(MonoCoreDriver.MonoGraphicsDevice, fileStream));
            }


            return newTexture;
        }
    }
}
