using ScorpionCore;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoScorpPlugin
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

        public byte[] Color { get; set; } = new byte[] { 0, 0, 0 };

        internal SpriteFont Font { get; }


        public T GetText<T>() where T : class
        {
            return Font as T;
        }
    }
}
