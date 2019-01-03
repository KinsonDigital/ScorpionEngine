using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for ColorList.xaml
    /// </summary>
    public partial class ColorList : UserControl
    {
        public ColorList()
        {
            //Test Data
            MyItems = new[]
            {
                new ColorItem { Color = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)) },
                new ColorItem { Color = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0)) }
            };

            InitializeComponent();

            Colors = new SolidColorBrush[]
            {
                new SolidColorBrush(Color.FromRgb(255, 0, 0)),
                new SolidColorBrush(Color.FromRgb(0, 255, 0))
            };
        }


        public ColorItem[] MyItems { get; set; }
    }
}
