using Microsoft.Xna.Framework.Content;
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
        public MonoContentLoader()
        {
            ContentRootDirectory = $"{GamePath}\\Content";
        }


        #region Props
        /// <summary>
        /// Gets the path to the executable game.
        /// </summary>
        public string GamePath { get; } = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

        /// <summary>
        /// Gets or sets the root directory for the game's content.
        /// </summary>
        public string ContentRootDirectory { get; set; }
        #endregion


        public T LoadTexture<T>(string textureName) where T : class, ITexture
        {
            var dir = Path.GetDirectoryName($@"Graphics\{textureName}");

            ITexture newTexture;

            newTexture = new MonoTexture(MonoCoreDriver.DriverContent.Load<Texture2D>($@"Graphics\{textureName}"));

            //using (var fileStream = File.OpenRead(fullPath))
            //{
            //    newTexture = new MonoTexture(Texture2D.FromStream(MonoCoreDriver.MonoGraphicsDevice, fileStream));
            //}


            return newTexture as T;
        }


        public T LoadText<T>(string name, string text) where T : class, IText
        {
            IText textItem = new MonoText(MonoCoreDriver.DriverContent.Load<SpriteFont>($@"Fonts\{name}"))
            {
                Text = text
            };

            return textItem as T;
        }
    }
}
