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
            var result = new Texture()
            {
                InternalTexture = _internalLoader.LoadTexture<ITexture>(textureName)
            };


            return result;
        }


        public GameText LoadText(string textName)
        {
            var result = new GameText
            {
                InternalText = _internalLoader.LoadText<IText>(textName)
            };


            return result;
        }
    }
}
