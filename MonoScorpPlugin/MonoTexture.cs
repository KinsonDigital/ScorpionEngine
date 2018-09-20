using ScorpionCore;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoScorpPlugin
{
    public class MonoTexture : ITexture
    {
        internal MonoTexture(Texture2D texture)
        {
            Texture = texture;
        }

        public int Width
        {
            get => Texture.Width;
            set { }
        }

        public int Height
        {
            get => Texture.Height;
            set { }
        }

        internal Texture2D Texture { get; }

        public T GetTexture<T>() where T : class
        {
            return Texture as T;
        }
    }
}
