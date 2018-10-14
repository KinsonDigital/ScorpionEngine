using ScorpionCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Graphics
{
    public class TextItem : IText
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] Color { get; set; }
        string IText.Text { get; set; }

        public T GetText<T>() where T : class
        {
            throw new NotImplementedException();
        }
    }
}
