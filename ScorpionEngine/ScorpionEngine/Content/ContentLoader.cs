using ScorpionCore;
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

        public ContentLoader()
        {

        }


        public string GamePath => _internalLoader.GamePath;


        public string ContentRootDirectory
        {
            get => _internalLoader.ContentRootDirectory;
            set => _internalLoader.ContentRootDirectory = value;
        }


        public T LoadText<T>(string name, string text) where T : class, IText
        {
            return _internalLoader.LoadText<T>(name, text);
        }


        public T LoadTexture<T>(string textureName) where T : class, ITexture
        {
            return _internalLoader.LoadTexture<T>(textureName);
        }
    }
}
