using Microsoft.Xna.Framework.Graphics;
using ScorpionEngine.Content;
using ScorpionEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoDriver
{
    public class MonoTexture : ITexture
    {
        private Texture2D _texture;


        public MonoTexture(Texture2D texture)
        {
            _texture = texture;
        }


        public int Width
        {
            get => _texture.Width;
            set { } 
        }

        public int Height
        {
            get => _texture.Height;
            set { }
        }

        public void GetTexture()
        {
            throw new NotImplementedException();
        }
    }
}
