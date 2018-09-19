using Microsoft.Xna.Framework.Graphics;
using ScorpionEngine.Content;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngineDriver
{
    //TODO: Add method docs
    public class MonoText : IText
    {
        public MonoText(string text)
        {
            Text = text;
        }


        internal MonoText(SpriteFont font)
        {
            Font = font;
        }

        //TODO: Look into make an interface that is shared among ITexture and IText.
        //Both of these interfaces are pretty much the same.

        public int Width
        {
            get => (int)Font.MeasureString("").X;
            set { }
        }

        public int Height
        {
            get => (int)Font.MeasureString("").X;
            set { }
        }

        public string Text { get; set; }

        public Color Color { get; set; } = Color.Black;

        internal SpriteFont Font { get; }


        public T GetText<T>() where T : class
        {
            return Font as T;
        }
    }
}
