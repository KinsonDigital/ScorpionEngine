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
    public class ContentLoader
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

        //public Text LoadText(string name, string text)
        //{
        //    return _internalLoader.LoadText<T>(name, text);
        //}


        public Texture LoadTexture(string textureName)
        {
            var loadedTexture = _internalLoader.LoadTexture<ITexture>(textureName);
            var result = new Texture(loadedTexture);


            return result;
        }
    }
}
