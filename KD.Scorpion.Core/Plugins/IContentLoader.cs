using KDScorpionCore.Graphics;
using PluginSystem;

namespace KDScorpionCore.Plugins
{
    public interface IContentLoader : IPlugin
    {
        /// <summary>
        /// Gets the path to the executable game.
        /// </summary>
        string GamePath { get; }

        /// <summary>
        /// Gets or sets the root directory for the game's content.
        /// </summary>
        string ContentRootDirectory { get; set; }


        T LoadTexture<T>(string name) where T : class, ITexture;


        T LoadText<T>(string name) where T : class, IText;
    }
}
