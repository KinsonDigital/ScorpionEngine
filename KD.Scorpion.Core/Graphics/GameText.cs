﻿namespace KDScorpionCore.Graphics
{
    public class GameText
    {
        #region Props
        internal IText InternalText { get; set; }

        public string Text
        {
            get => InternalText.Text;
            set
            {
                InternalText.Text = value == null ? "" : value;
            }
        }

        public int Width => InternalText.Width;

        public int Height => InternalText.Height;

        public GameColor Color
        {
            get =>InternalText.Color;
            set => InternalText.Color = value;
        }
        #endregion


        /// <summary>
        /// Concatenates the text of 2 <see cref="GameText"/> objects.
        /// </summary>
        /// <param name="textA">The first object.</param>
        /// <param name="textB">The second object.</param>
        /// <returns></returns>
        public static string operator +(GameText textA, GameText textB) => $"{textA.Text}{textB.Text}";


        public static string operator +(GameText textA, string textB) => $"{textA.Text}{textB}";


        public static string operator +(string textA, GameText textB) => $"{textA}{textB.Text}";
    }
}
