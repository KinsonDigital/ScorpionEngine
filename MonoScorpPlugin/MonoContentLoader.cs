using KDScorpionCore;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using KDScorpionCore.Plugins;
using KDScorpionCore.Graphics;

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


        public T LoadTexture<T>(string name) where T : class, ITexture
        {
            ITexture newTexture = new MonoTexture();
            newTexture.InjectData(MonoGame.Content.Load<Texture2D>($@"Graphics\{name}"));


            return newTexture as T;
        }


        public T LoadText<T>(string name) where T : class, IText
        {
            IText textItem = new MonoText();
            textItem.InjectData(MonoGame.Content.Load<SpriteFont>($@"Fonts\{name}"));


            return textItem as T;
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
