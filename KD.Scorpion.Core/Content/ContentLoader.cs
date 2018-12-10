using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;

namespace KDScorpionCore.Content
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
