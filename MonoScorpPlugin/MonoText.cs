using KDScorpionCore;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoScorpPlugin
{
    //TODO: Add method docs
    public class MonoText : IText
    {
        #region Props
        public int Width => Text == "" ? 0 : (int)Font.MeasureString(Text).X;

        public int Height => Text == "" ? 0 : (int)Font.MeasureString(Text).Y;

        public string Text { get; set; } = "";

        /// <summary>
        /// Gets or sets the color values of the text.
        /// Element 1: Red
        /// Element 2: Green
        /// Element 3: Blue
        /// Element 4: Alpha
        /// </summary>
        public byte[] Color { get; set; } = new byte[] { 255, 255, 255, 255 };

        internal SpriteFont Font { get; private set; }


        #endregion


        #region Public Methods
        public object GetData(string dataType)
        {
            throw new NotImplementedException();
        }


        public T GetText<T>() where T : class
        {
            return Font as T;
        }


        public void InjectData<T>(T data) where T : class
        {
            //If the incoming data is not a monogame sprite font, throw an exception
            if (data.GetType() != typeof(SpriteFont))
                throw new Exception($"Data getting injected into {nameof(MonoText)} is not of type {nameof(SpriteFont)}.  Incorrect type is {data.GetType().ToString()}");

            Font = data as SpriteFont;
        }
        #endregion
    }
}
