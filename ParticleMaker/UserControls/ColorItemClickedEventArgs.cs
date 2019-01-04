using System;
using System.Windows.Media;

namespace ParticleMaker.UserControls
{
    //TODO: Add docs to entire class
    public class ColorItemClickedEventArgs : EventArgs
    {
        #region Constructors
        public ColorItemClickedEventArgs(int id, Color color)
        {
            Id = id;
            Color = color;
        }
        #endregion


        #region Props
        public int Id { get; private set; }

        public Color Color { get; private set; }
        #endregion
    }
}
