using ScorpionCore;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScorpionCore.Plugins;

namespace MonoScorpPlugin
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
            ITexture newTexture;

            newTexture = new MonoTexture(MonoGame.Content.Load<Texture2D>($@"Graphics\{textureName}")) as ITexture;


            return newTexture as T;
        }


        public T LoadText<T>(string name, string text) where T : class, IText
        {
            //IText textItem = new MonoText(MonoGame.DriverContent.Load<SpriteFont>($@"Fonts\{name}"))
            //{
            //    Text = text
            //};

            return null;
            //return textItem as T;
        }

        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }

        public object GetData(string dataType)
        {
            throw new NotImplementedException();
        }
    }
}
