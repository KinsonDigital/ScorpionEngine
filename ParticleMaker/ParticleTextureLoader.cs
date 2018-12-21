using KDScorpionCore.Plugins;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Reflection;

namespace ParticleMaker
{
    public class ParticleTextureLoader : IContentLoader
    {
        private GraphicsDevice _grfxDevice;
        private const string CONTENT_DIR = "Content";


        #region Constructors
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
        public string GamePath { get; }

        public string ContentRootDirectory { get; set; }
        #endregion


        #region Public Methods
        public object GetData(string dataType)
        {
            throw new NotImplementedException();
        }


        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }


        T IContentLoader.LoadText<T>(string name)
        {
            throw new NotImplementedException();
        }


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
