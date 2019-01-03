using System;

namespace ParticleMaker.UserControls
{
    //TODO: Add docs to entire class
    public class ColorItemClickedEventArgs : EventArgs
    {
        public ColorItemClickedEventArgs(int id)
        {
            Id = id;
        }


        public int Id { get; private set; }
    }
}
