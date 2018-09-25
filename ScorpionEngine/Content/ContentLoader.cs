using ScorpionCore;
using ScorpionCore.Plugins;
using ScorpionEngine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Content
{
    public class ContentLoader : IContentLoader
    {
        IContentLoader _internalLoader;

        public ContentLoader(IContentLoader contentLoader)
        {
            _internalLoader = contentLoader;
        }


        public string GamePath => _internalLoader.GamePath;


        public string ContentRootDirectory
        {
            get => _internalLoader.ContentRootDirectory;
            set => _internalLoader.ContentRootDirectory = value;
        }

        public T GetData<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }

        public T LoadText<T>(string name, string text) where T : class, IText
        {
            return _internalLoader.LoadText<T>(name, text);
        }


        public T LoadTexture<T>(string textureName) where T: class, ITexture
        {
            var loadedTexture = _internalLoader.LoadTexture<ITexture>(textureName);
            var result = new Texture(loadedTexture);


            return result as T;
        }
    }
}
