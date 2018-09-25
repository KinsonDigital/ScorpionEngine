using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionCore
{
    //TODO: Add docs
    public interface IText
    {
        int Width { get; set; }

        int Height { get; set; }

        string Text { get; set; }

        byte[] Color { get; set; }

        T GetText<T>() where T : class;
    }
}
