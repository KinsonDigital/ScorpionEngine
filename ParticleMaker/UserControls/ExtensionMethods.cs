using System.Windows.Media;

namespace ParticleMaker.UserControls
{
    public static class ExtensionMethods
    {
        //TODO: Add docs
        public static SolidColorBrush ToNegativeBrush(this Color color)
        {
            var negativeForecolor = Color.FromArgb(255, (byte)(255 - color.R), (byte)(255 - color.G), (byte)(255 - color.B));

            return new SolidColorBrush(negativeForecolor);
        }


        //TODO: Add docs
        public static SolidColorBrush ToNegative(this SolidColorBrush brush)
        {
            return brush.Color.ToNegativeBrush();
        }
    }
}
