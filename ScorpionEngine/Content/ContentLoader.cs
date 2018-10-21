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


        internal ContentLoader(IContentLoader contentLoader)
        {
            _internalLoader = contentLoader;
        }


        public string GamePath => _internalLoader.GamePath;


        public string ContentRootDirectory
        {
            get => _internalLoader.ContentRootDirectory;
            set => _internalLoader.ContentRootDirectory = value;
        }


        public Texture LoadTexture(string textureName)
        {
            var loadedTexture = _internalLoader.LoadTexture<ITexture>(textureName);
            var result = new Texture(loadedTexture);


            return result;
        }


        public GameText LoadText(string textName)
        {
            var result = new GameText();
            result.InternalText = _internalLoader.LoadText<IText>(textName);


            return result;
        }
    }
}
