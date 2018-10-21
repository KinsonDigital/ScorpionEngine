using ScorpionCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Graphics
{
    public class Texture
    {
        internal Texture(ITexture texture)
        {
            InternalTexture = texture;
        }


        public Texture()
        {

        }


        internal ITexture InternalTexture { get; set; }


        public T GetTexture<T>() where T : class
        {
            return InternalTexture.GetTexture<T>();
        }


        public int Width
        {
            get => InternalTexture.Width;
            set => InternalTexture.Width = value;
        }

        public int Height
        {
            get => InternalTexture.Height;
            set => InternalTexture.Height = value;
        }
    }
}
