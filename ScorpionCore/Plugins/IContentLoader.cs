using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionCore.Plugins
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


        T LoadTexture<T>(string textureName) where T : class, ITexture;


        T LoadText<T>(string name, string text) where T : class, IText;
    }
}
