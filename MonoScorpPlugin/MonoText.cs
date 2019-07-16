using KDScorpionCore;
using KDScorpionCore.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoScorpPlugin
{
    /// <summary>
    /// The MonoGame <see cref="SpriteFont"/> text that can be rendered to the graphics surface.
    /// </summary>
    public class MonoText : IText
    {
        #region Props
        /// <summary>
        /// Gets or sets the text to be rendered to the graphics surface.
        /// </summary>
        public string Text { get; set; } = "";

        /// <summary>
        /// Gets the width of the text.
        /// </summary>
        public int Width => Text == "" ? 0 : (int)Font.MeasureString(Text).X;

        /// <summary>
        /// Gets the height of the text.
        /// </summary>
        public int Height => Text == "" ? 0 : (int)Font.MeasureString(Text).Y;

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        public GameColor Color { get; set; } = new GameColor(255, 255, 255, 255);

        /// <summary>
        /// Gets the internal MonoGame <see cref="SpriteFont"/>
        /// </summary>
        internal SpriteFont Font { get; private set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Injects any arbitrary data into the plugin for use.  Must be a class.
        /// </summary>
        /// <typeparam name="T">The type of data to inject.</typeparam>
        /// <param name="data">The data to inject.</param>
        /// <exception cref="Exceptoin">Thrown if the <paramref name="data"/> is not of type <see cref="SpriteFont"/>.</exception>
        public void InjectData<T>(T data) where T : class
        {
            //If the incoming data is not a monogame sprite font, throw an exception
            if (data.GetType() != typeof(SpriteFont))
                throw new Exception($"Data getting injected into {nameof(MonoText)} is not of type {nameof(SpriteFont)}.  Incorrect type is '{data.GetType().ToString()}'");

            Font = data as SpriteFont;
        }


        /// <summary>
        /// Gets the data as the given type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="option">Used to pass in options for the <see cref="GetData{T}(int)"/> implementation to process.</param>
        /// <typeparam name="T">The type of data to get.</typeparam>
        /// <exception cref="Exception">Thrown if the <paramref name="option"/> value is not the value of
        /// type '1' for the type <see cref="SpriteFont"/>.</exception>
        public T GetData<T>(int option) where T : class
        {
            if (option == 1)
                return Font as T;


            throw new Exception($"The option '{option}' is not valid. \n\nValid options are 1.");
        }
        #endregion
    }
}
