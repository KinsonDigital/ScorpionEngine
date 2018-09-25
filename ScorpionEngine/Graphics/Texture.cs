using ScorpionCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Graphics
{
    public class Texture : ITexture
    {
        private ITexture _internalTexture;


        internal Texture(ITexture texture)
        {
            _internalTexture = texture;
        }

        public Texture()
        {

        }


        public T GetTexture<T>() where T : class
        {
            return _internalTexture.GetTexture<T>();
        }


        public int Width
        {
            get => _internalTexture.Width;
            set => _internalTexture.Width = value;
        }

        public int Height
        {
            get => _internalTexture.Height;
            set => _internalTexture.Height = value;
        }
    }
}
