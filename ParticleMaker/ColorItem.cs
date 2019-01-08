using System.Windows.Media;

namespace ParticleMaker
{
    public class ColorItem
    {
        public int Id { get; set; }

        public SolidColorBrush ColorBrush { get; set; }


        public override string ToString()
        {
            return $"R:{ColorBrush.Color.R}, G:{ColorBrush.Color.G}, B:{ColorBrush.Color.B}";
        }
    }
}
