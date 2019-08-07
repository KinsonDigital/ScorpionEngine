using KDScorpionCore;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Microsoft.Xna.Framework.Content;
using KDScorpionCore.Plugins;
using KDScorpionCore.Graphics;

namespace MonoScorpPlugin
{
    /// <summary>
    /// Loads and unloads content using the MonoGame framework <see cref="ContentManager"/> class.
    /// </summary>
    public class MonoContentLoader : IContentLoader
    {
        #region Constructors
        public MonoContentLoader() => ContentRootDirectory = $@"{GamePath}\Content";
        #endregion


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


        #region Public Methods
        /// <summary>
        /// Loads a texture that has the given <paramref name="name"/>.
        /// </summary>
        /// <typeparam name="T">The type of texture to render.</typeparam>
        /// <param name="name">The name of the texture object to render.</param>
        /// <returns></returns>
        public T LoadTexture<T>(string name) where T : class, ITexture
        {
            ITexture newTexture = new MonoTexture();
            newTexture.InjectData(MonoGame.Content.Load<Texture2D>($@"Graphics\{name}"));


            return newTexture as T;
        }


        /// <summary>
        /// Loads a text objec to render that has the given <paramref name="name"/>.
        /// </summary>
        /// <typeparam name="T">The type of text object to render.</typeparam>
        /// <param name="name">The name of the text object to render.</param>
        /// <returns></returns>
        public T LoadText<T>(string name) where T : class, IText
        {
            IText textItem = new MonoText();
            textItem.InjectData(MonoGame.Content.Load<SpriteFont>($@"Fonts\{name}"));


            return textItem as T;
        }


        /// <summary>
        /// Gets the data as the given type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="option">Used to pass in options for the <see cref="GetData{T}(int)"/> implementation to process.</param>
        /// <typeparam name="T">The type of data to get.</typeparam>
        /// <returns></returns>
        public T GetData<T>(int option) where T : class => throw new NotImplementedException();


        /// <summary>
        /// Injects any arbitrary data into the plugin for use.  Must be a class.
        /// </summary>
        /// <typeparam name="T">The type of data to inject.</typeparam>
        /// <param name="data">The data to inject.</param>
        public void InjectData<T>(T data) where T : class => throw new NotImplementedException();
        #endregion
    }
}
