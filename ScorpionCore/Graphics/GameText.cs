namespace ScorpionCore.Graphics
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
                InternalText.Text = value;
            }
        }

        public int Width
        {
            get => InternalText.Width;
        }

        public int Height
        {
            get => InternalText.Height;
        }

        public GameColor Color
        {
            get => new GameColor(InternalText.Color[0], InternalText.Color[1], InternalText.Color[2], InternalText.Color[3]);
            set
            {
                InternalText.Color[0] = value.Red;
                InternalText.Color[1] = value.Green;
                InternalText.Color[2] = value.Blue;
                InternalText.Color[3] = value.Alpha;
            }
        }
        #endregion


        public static string operator +(GameText textA, GameText textB)
        {
            return $"{textA}{textB}";
        }


        public static string operator +(GameText textA, string textB)
        {
            return $"{textA}{textB}";
        }


        public static string operator +(string textA, GameText textB)
        {
            return $"{textA}{textB}";
        }
    }
}
