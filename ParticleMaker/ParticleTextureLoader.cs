﻿using KDScorpionCore.Plugins;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Reflection;

namespace ParticleMaker
{
    /// <summary>
    /// Loads particle textures for rendering.
    /// </summary>
    public class ParticleTextureLoader : IContentLoader
    {
        #region Fields
        private GraphicsDevice _grfxDevice;
        private const string CONTENT_DIR = "Content";
        #endregion


        #region Constructors
        //TODO: Look into injecting the grfxDevice param into this class via the InjectData method.
        //The code inside the method will be implemented into the ContentDirService class that will
        //be injected into this class.
        /// <summary>
        /// Creates a new instance of a <see cref="ParticleTextureLoader"/>.
        /// </summary>
        /// <param name="grfxDevice">The graphics device to load the textures into.</param>
        public ParticleTextureLoader(GraphicsDevice grfxDevice)
        {
            _grfxDevice = grfxDevice;

            ContentRootDirectory = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\{CONTENT_DIR}";

            if (!Directory.Exists(ContentRootDirectory))
            {
                Directory.CreateDirectory(ContentRootDirectory);

                throw new Exception("There is no content in the directory to load.");
            }
            else
            {
                //Check if there are any files, it not then throw an exception
                if (Directory.GetFiles(ContentRootDirectory).Length <= 0)
                {
                    throw new Exception("There is no content in the directory to load.");
                }
            }
        }
        #endregion


        #region Props
        /// <summary>
        /// The path to the game executable.
        /// </summary>
        public string GamePath { get; }

        /// <summary>
        /// The root directory where the texture to load is located.
        /// </summary>
        public string ContentRootDirectory { get; set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Gets the data using the given <paramref name="dataType"/>.
        /// </summary>
        /// <param name="dataType">The type of data to return.</param>
        /// <returns></returns>
        public object GetData(string dataType)
        {
            throw new NotImplementedException();
        }

        //TODO: Use this to inject the graphics device
        /// <summary>
        /// Injects the given <paramref name="data"/> into the <see cref="ParticleTextureLoader"/> for use.
        /// </summary>
        /// <typeparam name="T">The type of data to inject.</typeparam>
        /// <param name="data">The data to inject.</param>
        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Loads the text of type <typeparamref name="T"/> that matches the given <paramref name="name"/>.
        /// </summary>
        /// <typeparam name="T">The type of text data to load.</typeparam>
        /// <param name="name">The name of the text item to load.</param>
        /// <returns></returns>
        T IContentLoader.LoadText<T>(string name)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Loads the texture of type <typeparamref name="T"/> that matches the given <paramref name="name"/>.
        /// </summary>
        /// <typeparam name="T">The type of texture to load.</typeparam>
        /// <param name="name">The name of the texture to load.</param>
        /// <returns></returns>
        T IContentLoader.LoadTexture<T>(string name)
        {
            Texture2D texture;

            using (var file = File.OpenRead($@"{ContentRootDirectory}\{name}.png"))
            {
                texture = Texture2D.FromStream(_grfxDevice, file);
            }


            return new ParticleTexture(texture) as T;
        }
        #endregion
    }
}
