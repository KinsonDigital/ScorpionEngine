using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using System.Diagnostics.CodeAnalysis;

namespace KDScorpionCore.Content
{
    //TODO: Add docs to code
    public class ContentLoader
    {
        #region Fields
        private readonly IContentLoader _internalLoader;
        #endregion


        #region Constructors
        internal ContentLoader(IContentLoader contentLoader) => _internalLoader = contentLoader;


        [ExcludeFromCodeCoverage]
        public ContentLoader() => _internalLoader = CorePluginSystem.Plugins.EnginePlugins.LoadPlugin<IContentLoader>();
        #endregion


        #region Props
        public string GamePath => _internalLoader.GamePath;


        public string ContentRootDirectory
        {
            get => _internalLoader.ContentRootDirectory;
            set => _internalLoader.ContentRootDirectory = value;
        }
        #endregion


        #region Public Methods
        public Texture LoadTexture(string textureName) => new Texture(_internalLoader.LoadTexture<ITexture>(textureName));


        public GameText LoadText(string textName) => new GameText()
            {
                InternalText = _internalLoader.LoadText<IText>(textName)
            };
        #endregion
    }
}
