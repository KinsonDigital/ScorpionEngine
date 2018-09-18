using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Content
{
    public interface IContentLoader
    {
        ITexture LoadTexture(string textureName);
    }
}
